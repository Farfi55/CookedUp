using ThinkEngine.Mappers.BaseMappers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine.Mappers
{
    internal class ASPBoolMapper : BasicTypeMapper
    {
        #region SINGLETON FEATURES

        private static IDataMapper _instance;
        public static IDataMapper Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new ASPBoolMapper();

                }
                return _instance;
            }
        }


        private ASPBoolMapper()
        {
            _operationList = new List<Operation>() { Newest, Oldest, Always, Count, AtLeast, Conjunction, Disjunction };
            _supportedTypes = new List<Type> { typeof(bool), typeof(bool) };
            _convertingType = typeof(bool);
        }
        #endregion

        internal static object Conjunction(IList values, object value = null, int counter = 0)
        {
            bool conj = true;
            foreach (bool v in values)
            {
                conj &= v;
            }
            return conj;
        }
        internal static object Disjunction(IList values, object value = null, int counter = 0)
        {
            bool conj = true;
            foreach (bool v in values)
            {
                conj |= v;
            }
            return conj;
        }
        public override string BasicMap(object currentObject)
        {
            if (!SupportedTypes.Contains(currentObject.GetType()))
            {
                Debug.LogError("Type " + currentObject.GetType() + " is not supported as Sensor");
            }
            return ((bool)currentObject + "").ToLower();
        }

        public override Type GetAggregationTypes(Type type)
        {
            return typeof(BoolOperations);
        }

    }
}