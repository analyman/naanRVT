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

        private void __TransformB(double off, double radius)
        {
            if(this.y > off + radius){
                return;
            }
            double extraDis = (Math.PI / 2 - 1) * radius;
            if(this.y < off - extraDis){
                this.z += (extraDis + this.y - off - radius);
                this.y = off;
                return;
            }
            double offDis = off + radius - this.y;
            double angle = offDis / radius;
            double complementAngle = (Math.PI / 2) - angle;
            this.z -= (radius - Math.Sin(complementAngle) * radius);
            this.y = off + radius * (1 - Math.Cos(complementAngle));
            return;
        }

        private void swapxy()
        {
            double temp = this.x;
            this.x = this.y;
            this.y = temp;
            return;
        }

        public void TransformX(double RX, double RY, double RNX, double RNY)
        {
            this.__TransformB(RY, RNY);
            this.swapxy();
            this.__TransformB(RX, RNX);
            this.swapxy();
            return;
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

        public static CSPoint operator+(CSPoint fir, CSPoint sec)
        {
            return new CSPoint(fir.x + sec.x, fir.y + sec.y, fir.z + sec.z);
        }

        public static CSPoint operator-(CSPoint fir, CSPoint sec)
        {
            return new CSPoint(fir.x - sec.x, fir.y - sec.y, fir.z - sec.z);
        }
        
        public override int GetHashCode()
        {
            int xseed = (int)Math.Sin(this.x);
            int yseed = (int)Math.Cosh(this.y);
            int zseed = (int) this.z;
            return (xseed * yseed) / (zseed % 0x20);
        }

        public override string ToString()
        {
            return "CSPoint(X = " + this.x.ToString() + ", Y = " +
                        this.y.ToString() + ", Z = " + this.z.ToString() + ")";
        }

        public static CSPoint CSPointOrigin = new CSPoint(0, 0);
    }
}
