using System;
using System.Collections.Generic;
using NaaN;

namespace NaaN.Test
{
    class utilsTest
    {
        public static void utilsTestMain()
        {
            utils_multiLevelMapTest();
//            utils_twoLevelMapTest();
        }
        public static int utils_twoLevelMapTest()
        {
            List<List<int>> s = new List<List<int>> {new List<int> { 1, 2, 3}, new List<int> {8, 9, 10}};
            List<List<double>> r = utils.twoLevelMap<int, double>(x => x * 0.77, s);
            utils.twoLevelMap<double, int>((double x) => {Console.WriteLine(x.ToString()); return 0;}, r);
            return 0;
        }

        public static int utils_multiLevelMapTest()
        {
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
    }
}
