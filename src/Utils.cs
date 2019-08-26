using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace NaanRevit
{
    /// <summary>
    /// The purpose of this class is to provide some useful
    /// function to increase efficient of developing revit addin
    /// </summary>
    public class Utils
    {
        public delegate bool ElementTest(Element e);
        public delegate WHAT ElementTo<WHAT>(Element e);
        /// <summary>
        /// Test whether a revit element contain specified parameter of which name
        /// equal with <paramref name="p">name</paramref>
        /// </summary>
        /// <param name="e">Element be tested</param>
        /// <param name="p">Name of parameter definition</param>
        /// <returns></returns>
        public static bool hasParameter(Element e, string p)
        {
            ParameterSet allParameter = e.Parameters;
            foreach(Parameter para in allParameter)
            {
                if (para.Definition.Name == p)
                    return true;
            }
            return false;
        }

        public static List<Element> ElementFilter(ICollection<Element> elist, ElementTest testDelegate)
        {
            List<Element> ret = new List<Element>();
            foreach(Element elem in elist)
            {
                if (testDelegate(elem))
                    ret.Add(elem);
            }
            return ret;
        }

        public static List<RT> ElementMap<RT>(ICollection<Element> elist, ElementTo<RT> toDelegate)
        {
            List<RT> ret = new List<RT>();
            foreach(Element elem in elist)
                ret.Add(toDelegate(elem));
            return ret;
        }
    }
}
