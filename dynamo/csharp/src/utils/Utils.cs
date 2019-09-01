using System;
using Autodesk.Revit.Attributes;
using System.Linq;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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

        public static T DeepCopy<T>(T obj)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)bformatter.Deserialize(ms);
            }
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

        public static object multiLevelMapList(Func<object, object> func, object obj)
        {
            Tuple<object, Type> auxReturn = multiLevelMap_Aux(func, obj);
            return auxReturn.Item1;
        }

        private static Tuple<object, Type> multiLevelMap_Aux(Func<object, object> func, object obj)
        {
            System.Collections.IEnumerable xyz = obj as System.Collections.IEnumerable;
            if(null == xyz){
                object x = func(obj);
                return new Tuple<object, Type>(x, x.GetType());
            }
            System.Collections.ArrayList arrayListret = new System.Collections.ArrayList(); 
            foreach (object i in xyz){
                arrayListret.Add(multiLevelMap_Aux(func, i));
            }
            Type listGeneric = typeof(System.Collections.Generic.List<>);
            Tuple<object, Type> firstret = arrayListret[0] as Tuple<object, Type>;
            if(firstret == null){
                throw new Exception("WA, Ok it's wrong");
            }
            Type returnType = listGeneric.MakeGenericType(firstret.Item2);
            System.Collections.IList retVal = (System.Collections.IList)System.Activator.CreateInstance(returnType);
            foreach (Tuple<object, Type> j in arrayListret){
                retVal.Add(j.Item1);
            }
            return new Tuple<object, Type>(retVal, retVal.GetType());
        }
    }
}
