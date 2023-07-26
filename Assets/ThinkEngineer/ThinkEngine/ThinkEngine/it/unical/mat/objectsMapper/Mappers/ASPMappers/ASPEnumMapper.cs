using ThinkEngine.Mappers.BaseMappers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine.Mappers
{
    internal class ASPEnumMapper : BasicTypeMapper
    {
        #region SINGLETON FEATURES
        private static IDataMapper _instance;
        public static IDataMapper Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new ASPEnumMapper();

                }
                return _instance;
            }
        }
        private ASPEnumMapper()
        {
            _operationList = new List<Operation>() { Newest, Oldest,Always, Count, AtLeast};
            _supportedTypes = new List<Type> { typeof(Enum) };
            _convertingType = typeof(Enum);

        }
        #endregion

        public override string BasicMap(object currentObject)
        {
            if (!SupportedTypes.Contains(currentObject.GetType()))
            {
                Debug.LogError("Type " + currentObject.GetType() + " is not supported as Sensor");
            }
            return "\"" + currentObject+"\"";
        }

        public override Type GetAggregationTypes(Type type)
        {
            return typeof(StringOperations);
        }
    }
}