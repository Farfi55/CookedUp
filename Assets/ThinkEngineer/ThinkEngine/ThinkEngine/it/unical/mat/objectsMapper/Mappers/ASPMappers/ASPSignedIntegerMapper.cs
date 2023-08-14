using ThinkEngine.Mappers.BaseMappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine.Mappers
{
    internal class ASPSignedIntegerMapper : BasicTypeMapper
    {
        private static object Avg(IList values, object value = null, int counter = 0)
        {
            if (values.Count == 0)
            {
                Debug.LogError("No values to compute avg on.");
            }
            long avg = 0;
            foreach (long d in values)
            {
                avg += d;
            }
            avg /= (long)values.Count;
            return avg;
        }
        private static object Min(IList values, object value = null, int counter = 0)
        {
            long min = long.MaxValue;
            foreach (long v in values)
            {
                if (v < min)
                {
                    min = v;
                }
            }
            return min;
        }
        private static object Max(IList values, object value = null, int counter = 0)
        {
            long max = long.MinValue;
            foreach (long v in values)
            {
                if (v > max)
                {
                    max = v;
                }
            }
            return max;
        }
        #region SINGLETON FEATURES

        private static IDataMapper _instance;
        public static IDataMapper Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new ASPSignedIntegerMapper();

                }
                return _instance;
            }
        }
        private ASPSignedIntegerMapper()
        {
            _operationList = new List<Operation>() { Newest, Oldest, Always, Count, AtLeast, Max, Min, Avg };
            _supportedTypes= new List<Type> { typeof(sbyte), typeof(short), typeof(int), typeof(long) };
            _convertingType = typeof(long);
        }
        #endregion

        public override string BasicMap(object currentObject)
        {
            if (!SupportedTypes.Contains(currentObject.GetType()))
            {
                Debug.LogError("Type " + currentObject.GetType() + " is not supported as Sensor");
            }
            return "" + currentObject + "";
        }

        public override Type GetAggregationTypes(Type type)
        {
            return typeof(NumericOperations);
        }
    }
}
