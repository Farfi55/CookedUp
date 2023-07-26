using ThinkEngine.Mappers.BaseMappers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine.Mappers
{
    internal class ASPUnsignedIntegerMapper : BasicTypeMapper
    {
        private static object Avg(IList values, object value = null, int counter = 0)
        {
            if (values.Count == 0)
            {
                Debug.LogError("No values to compute avg on.");
            }
            ulong avg = 0;
            foreach (ulong d in values)
            {
                avg += d;
            }
            avg /= (ulong)values.Count;
            return avg;
        }
        private static object Min(IList values, object value = null, int counter = 0)
        {
            ulong min = ulong.MaxValue;
            foreach (ulong v in values)
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
            ulong max = ulong.MinValue;
            foreach (ulong v in values)
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
                    _instance = new ASPUnsignedIntegerMapper();

                }
                return _instance;
            }
        }
        private ASPUnsignedIntegerMapper() 
        {
            _operationList = new List<Operation>() { Newest, Oldest, Always, Count, AtLeast, Max, Min, Avg };
            _supportedTypes = new List<Type> { typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) };
            _convertingType = typeof(ulong);
        }
        #endregion
        public override string BasicMap(object currentObject)
        {
            if (!SupportedTypes.Contains(currentObject.GetType()))
            {
                Debug.LogError("Type " + currentObject.GetType() + " is not supported as Sensor");
            }
            return ""+currentObject+"";
        }
        public override Type GetAggregationTypes(Type type)
        {
            return typeof(NumericOperations);
        }
    }
}