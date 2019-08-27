using System;
using System.Collections.Generic;
using NaaN.dynamo.Geometry;

namespace NaaN.Test
{
    class geometryTest
    {
        public static void geometryTestMain()
        {
            DiamondPointsTest();
//            utils_twoLevelMapTest();
        }
        public static int DiamondPointsTest(){
            DiamondPoints dx = new DiamondPoints(8, 10, 200, 400);
            Console.WriteLine(dx.ToString());
            return 0;
        }
    }
}
