using ThinkEngine.Mappers.BaseMappers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine.Mappers
{
    internal class ASPFloatingPointMapper : BasicTypeMapper
    {
        private static object Avg(IList values, object value = null, int counter = 0)
        {
            if (values.Count == 0)
            {
                Debug.LogError("No values to compute avg on.");
            }
            double avg = 0;
            foreach (double d in values)
            {
                avg += d;
            }
            avg /= values.Count;
            return avg;
        }
        private static object Min(IList values, object value = null, int counter = 0)
        {
            double min = double.MaxValue;
            foreach (double v in values)
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
            double max = double.MinValue;
            foreach (double v in values)
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
                    _instance = new ASPFloatingPointMapper();

                }
                return _instance;
            }
        }
        private ASPFloatingPointMapper() 
        {
            _operationList = new List<Operation>() { Newest, Oldest, Always, Count, AtLeast, Max, Min, Avg };
            _supportedTypes = new List<Type> { typeof(double), typeof(float) };
            _convertingType = typeof(double);
        }
        #endregion

        public override string BasicMap(object currentObject)
        {
            if (!SupportedTypes.Contains(currentObject.GetType()))
            {
                Debug.LogError("Type " + currentObject.GetType() + " is not supported as Sensor");
            }
            return "\"" + currentObject + "\"";
        }

        public override Type GetAggregationTypes(Type type)
        {
            return typeof(NumericOperations);
        }
    }
}