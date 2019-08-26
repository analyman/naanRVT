using System;
using Autodesk.Revit.Attributes;
using System.Linq;
using System.Collections.Generic;

namespace NaaN
{
    public static class utils
    {
        public static List<List<R>> twoLevelMap<T, R>(Func<T, R> func, IEnumerable<IEnumerable<T>> enumObj)
        {
            List<List<R>> ret = new List<List<R>>();
            foreach (IEnumerable<T> i in enumObj){
                ret.Add(new List<R>(System.Linq.Enumerable.Select<T, R>(i, func)));
            }
            return ret;
        }

        public static object multiLevelMap(Func<object, object> func, object obj)
        {
            System.Collections.IEnumerable xyz = obj as System.Collections.IEnumerable;
            if(null == xyz){
                return func(obj);
            }
            System.Collections.ArrayList listret = new System.Collections.ArrayList(); 
            foreach (object i in xyz){
                listret.Add(multiLevelMap(func, i));
            }
            return listret;
        }
    }
}
