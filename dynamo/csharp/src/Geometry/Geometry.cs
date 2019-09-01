using MathNet.Numerics.LinearAlgebra;
using System;
using MathNet.Numerics;
using System.Numerics;

/// <summary>
/// basic operation for all of Geometry
/// </summary>

namespace NaaN.dynamo.Geometry
{
    public class AFTransform
    {
        private double[,] transM;
        private double[]  transV;
        private bool isMatrixValid = false;
        private Matrix<double> mathnetMatrix;
        private Vector<double> mathnetVector;

        private event EventHandler TransformChanged;
        private void OnTransformChanged()
        {
            EventHandler xhandle = TransformChanged;
            xhandle?.Invoke(this, EventArgs.Empty);
        }

        private double [,] TransM {
            get {return transM;}
            set {transM = value; OnTransformChanged();}
        }
        private double [] TransV {
            get {return transV;}
            set {transV = value; OnTransformChanged();}
        }

        private Matrix<double> MathnetMatrix {
            get {
                updateMathNetMatrix();
                return mathnetMatrix;
            }
            set {
                this.mathnetMatrix = value;
            }
        }

        private Vector<double> MathnetVector {
            get {
                updateMathNetMatrix();
                return mathnetVector;
            }
            set {
                this.mathnetVector = value;
            }
        }

        public  bool IsInversable {
            get {
                if(this.MathnetMatrix.Rank() == 3)
                {
                    return true;
                }
                return false;
            }
        }

        private void initializeEventHandles()
        {
            this.TransformChanged += (object s, EventArgs e) =>
            {
                ((AFTransform)s).isMatrixValid = false;
            };
        }

        public AFTransform()
        {
            TransM = new double[,] {
                {1, 0, 0},
                {0, 1, 0},
                {0, 0, 1}
            };
            TransV = new double[] {0, 0, 0};
        }

        public AFTransform(double[,] M, double [] V = null)
        {
            for(int i = 0; i<2; i++)
            {
                if(M.GetLength(i) != 3)
                {
                    throw new Exception("Invalid 3x3 matrix");
                }
            }

            if (null == V)
            {
                TransV = new double[] { 0, 0, 0 };
            }
            else if (V.Length != 3)
            {
                throw new Exception("Invalid 3 dimension vector");
            }
            else
            {
                TransV = utils.DeepCopy<double[]>(V);
            }

            TransM = utils.DeepCopy<double[,]>(M);
        }

        private void updateMathNetMatrix()
        {
            if (this.isMatrixValid)
                return;
            this.mathnetMatrix = Matrix<double>.Build.DenseOfArray(this.TransM);
            this.mathnetVector = Vector<double>.Build.DenseOfArray(this.TransV);
            this.isMatrixValid = true;
        }

        public AFTransform InverseTransform()
        {
            if(!IsInversable){
                throw new Exception("can't get inverse transformation, because it's not inversable");
            }
            Matrix<double> revM = this.mathnetMatrix.Inverse();
            Vector<double> revV =  - revM * this.mathnetVector;
            double[,] xarray = new double[3, 3];
            for(int i = 0; i<3; i++){
                for(int j = 0; j<3; j++){
                    xarray[j, i] = revM.Storage[j, i];
                }
            }
            revM.Storage.AsArray();
            return new AFTransform(
                xarray,
                revV.Storage.AsArray()
            );
        }

        public override string ToString()
        {
            string ret = "Matrix Part:\n";
            ret += this.MathnetMatrix.ToString();
            ret += "\nVector part:\n";
            ret += this.MathnetVector.ToString();
            return ret;
        }

        public static readonly AFTransform OriginTransform = new AFTransform();
        public static AFTransform ByTranslate(double[] off)
        {
            return new AFTransform(new double[3,3]{{1, 0, 0}, {0, 1, 0}, {0, 0, 1}}, off);
        }

        public static AFTransform ByCSPoint(CSPoint pt)
        {
            return new AFTransform(new double[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } },
                                   new double[3] { pt.X, pt.Y, pt.Z });
        }

        public static AFTransform ByCSVector(CSVector pt)
        {
            return new AFTransform(new double[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } },
                                   new double[3] { pt.X, pt.Y, pt.Z });
        }
    }

    public interface IGeometry 
    {
        void Transform(AFTransform aft);
    }
}