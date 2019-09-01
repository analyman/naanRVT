using System;
using System.Collections.Generic;
using NaaN.dynamo.Geometry;

namespace NaaN.Test
{
    class geometryTest
    {
        public static void geometryTestMain()
        {
//            DiamondPointsTest();
            AFTransformTest();
        }

        public static int DiamondPointsTest(){
            DiamondPoints dx = new DiamondPoints(8, 10, 200, 400);
            Console.WriteLine(dx.ToString());
            return 0;
        }

        public static int AFTransformTest()
        {
            var x = new AFTransform(new double[3,3]{
                {1, 2, 4},
                {2, 3, 5},
                {2, 2, 1.5}
            });
            Console.WriteLine(x.ToString());
            var y = x.InverseTransform();
            Console.WriteLine(y.ToString());
            return 0;
        }
    }
}
