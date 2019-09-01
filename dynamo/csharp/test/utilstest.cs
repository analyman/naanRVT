using System;
using System.Collections.Generic;
using NaaN;

using NaaN.dynamo.Geometry;

namespace NaaN.Test
{
    class utilsTest
    {
        public static void utilsTestMain()
        {
            utils_multiLevelMapListTest2();
//            utils_twoLevelMapTest();
//            utils_multiLevelMapTest();
        }
        public static int utils_twoLevelMapTest()
        {
            List<List<int>> s = new List<List<int>> {new List<int> { 1, 2, 3, 8, 10, 20}, new List<int> {8, 9, 10}};
            List<List<double>> r = utils.twoLevelMap<int, double>(x => x * 0.77, s);
            utils.twoLevelMap<double, int>((double x) => {Console.WriteLine(x.ToString()); return 0;}, r);
            return 0;
        }

        public static int utils_multiLevelMapTest()
        {
            Console.WriteLine("multiLevelMap() Test:");
            List<List<int>> s = new List<List<int>> {new List<int> { 1, 2, 3}, new List<int> {8, 9, 10}};
            var r = utils.multiLevelMap(
                x =>
                {
                    Nullable<int> y = x as Nullable<int>;
                    if (y.HasValue)
                        return y.Value * 0.77;
                    return 0;
                }, s);
            utils.multiLevelMap(x => { Console.WriteLine(x.ToString()); return 0; }, r);
            return 0;
        }

        public static int utils_multiLevelMapListTest()
        {
            Console.WriteLine("multiLevelMapList() Test:");
            List<List<int>> s = new List<List<int>> {new List<int> { 1, 2, 3}, new List<int> {8, 9, 10}};
            var r = utils.multiLevelMapList(
                x =>
                {
                    Nullable<int> y = x as Nullable<int>;
                    if (y.HasValue)
                        return y.Value * 0.77;
                    return 0;
                }, s);
            utils.multiLevelMapList(x => { Console.WriteLine(x.ToString()); return 0; }, r);
            Console.WriteLine("Type of return value: " + r.GetType().ToString());
            return 0;
        }

        public static int utils_multiLevelMapListTest2()
        {
            Console.WriteLine("multiLevelMapList() Test:");
            DiamondPoints x = new DiamondPoints(8, 8, 200, 400);
            x.TransformPoints(200, 200);
            var y = utils.multiLevelMapList(lmn => {return lmn;}, x.AllPanels);
            utils.multiLevelMapList(z => { Console.WriteLine(z.ToString()); return 0; }, x.AllPanels);
            Console.WriteLine("Type of return value: " + y.GetType().ToString());
            return 0;
        }

    }
}
