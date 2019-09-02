using MathNet.Numerics.LinearAlgebra;
using System;
using MathNet.Numerics;
using System.Numerics;

/// <summary>
/// basic operation for all of Geometry
/// </summary>

namespace NaaN.dynamo.Geometry
{
    public class AFTransform: IGeometry
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
            double[,] xarray = geometryUtils.MathMatrixToArray(revM);
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

        public static AFTransform ByRotateAroundVector(CSVector vec, double theta)
        {
            double[,] rotateTransformArray = geometryUtils.RotateThetaToMatrix(theta);

            Vector<double> mathnetV = Vector<double>.Build.DenseOfArray(new double[3]{vec.X, vec.Y, vec.Z});
            double[,] A = geometryUtils.VectorToStardardZTransform(mathnetV);
            double[,] AI = geometryUtils.inverseArrayMatrix(A);
            return (new AFTransform(A)) * (new AFTransform(rotateTransformArray)) * (new AFTransform(AI));
        }

        public static AFTransform operator*(AFTransform af1, AFTransform af2)
        {
            Matrix<double> af3Matrix = af1.mathnetMatrix * af2.mathnetMatrix;
            Vector<double> af3Vector = af2.mathnetMatrix * af1.mathnetVector + af2.mathnetVector;
            return new AFTransform(geometryUtils.MathMatrixToArray(af3Matrix), af3Vector.Storage.AsArray());
        }

        public  void AffineTransform(AFTransform aft)
        {
             AFTransform result =  this * aft;
             this.transM = result.transM;
             this.transV = result.transV;
        }
    }

    public interface IGeometry 
    {
        void AffineTransform(AFTransform aft);
    }

    internal static class geometryUtils
    {
        public static double[,] RotateThetaToMatrix(double theta)
        {
            double[,] ret = new double[3,3]{
                {Math.Cos(theta), Math.Sin(theta), 0},
                {-Math.Sin(theta), Math.Cos(theta), 0},
                {0, 0, 1}
            };
            return ret;
        }

        public static double[,] VectorToStardardZTransform(Vector<double> vec)
        {
            vec = vec / vec.L2Norm();
            double[] vec_array = vec.Storage.AsArray();
            double[] zero_array = new double[3]{0, 0, 0};

            Matrix<double> problem = Matrix<double>.Build.DenseOfRowArrays(new double[][] {vec_array, zero_array, zero_array});
            Vector<double>[] kernels = problem.Kernel();

            if(kernels.Length != 2)
            {
                throw new Exception("It's impossible, except parameter vec is zero");
            }

            double[] v1 = kernels[0].Storage.AsArray();
            double[] v2 = kernels[1].Storage.AsArray();

            return new double[3,3]
            {
                {vec_array[0], vec_array[1], vec_array[2]},
                {v1[0], v1[1], v1[2]},
                {v2[0], v2[1], v2[2]}
            };
        }

        public static double[,] inverseArrayMatrix(double[,] darray)
        {
            if(darray.Length == 0 || darray.GetLength(0) != darray.GetLength(1))
            {
                throw new ArgumentException("Expect a square two dimension array");
            }
            Matrix<double> x = Matrix<double>.Build.DenseOfArray(darray);
            Matrix<double> x_inv = x.Inverse();
            return MathMatrixToArray(x_inv);
        }

        public static double[,] MathMatrixToArray(Matrix<double> x)
        {
            double[,] xarray = new double[x.RowCount, x.ColumnCount];
            for(int i = 0; i<x.ColumnCount; i++){
                for(int j = 0; j<x.RowCount; j++){
                    xarray[j, i] = x.Storage[j, i];
                }
            }
            return xarray;
        }

    }
}