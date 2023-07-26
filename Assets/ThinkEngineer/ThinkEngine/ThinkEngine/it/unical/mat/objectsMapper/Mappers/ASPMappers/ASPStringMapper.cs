using ThinkEngine.Mappers.BaseMappers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine.Mappers
{
    internal class ASPStringMapper : BasicTypeMapper
    {
        #region SINGLETON FEATURES
        private static IDataMapper _instance;
        public static IDataMapper Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new ASPStringMapper();

                }
                return _instance;
            }
        }
        private ASPStringMapper() 
        {
            _operationList = new List<Operation>() { Newest, Oldest, Always, Count, AtLeast };
            _supportedTypes = new List<Type> { typeof(string) };
            _convertingType = typeof(string);
        }
        #endregion

        public override string BasicMap(object currentObject)
        {
            if (!SupportedTypes.Contains(currentObject.GetType()))
            {
                Debug.LogError("Type " + currentObject.GetType() + " is not supported as Sensor");
            }
            return "\""+currentObject+"\"";
        }

        public override Type GetAggregationTypes(Type type)
        {
            return typeof(StringOperations);
        }
    }
}