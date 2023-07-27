using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThinkEngine.Mappers;
using UnityEngine;
using System.Reflection;

namespace ThinkEngine
{
    public abstract class Sensor
    {
        protected bool invariant;
        protected bool first=true; //GMDG private
        protected GameObject gameObject; //GMDG default (private)
        //MonoBehaviourSensorsManager manager; //GMDG not necessary
        protected SensorConfiguration configuration;
        private MyListString property;
        private List<IInfoAndValue> _propertyInfo;
        protected bool ready; //GMDG private, default value false

        internal List<IInfoAndValue> PropertyInfo
        {
            get
            {
                if (_propertyInfo == null)
                {
                    _propertyInfo = new List<IInfoAndValue>();
                }
                return _propertyInfo;
            }
        }
        protected string _mapping;
        internal string Mapping
        {
            get
            {
                return _mapping;
            }
        }
        public int count = 0;

        public void Configure(InstantiationInformation information, string mapping)
        {
            gameObject = information.instantiateOn;
            //manager = gameObject.GetComponent<MonoBehaviourSensorsManager>();
            configuration = (SensorConfiguration)information.configuration;
            property = new MyListString(information.propertyHierarchy.myStrings);
            PropertyFeatures features = information.configuration.PropertyFeaturesList.Find(x => x.property.Equals(property));
            PropertyInfo.AddRange(information.hierarchyInfo);
            if (information.instantiateOn.GetComponent<IndexTracker>() == null)
            {
                information.instantiateOn.AddComponent<IndexTracker>();
            }
            int index = information.instantiateOn.GetComponent<IndexTracker>().CurrentIndex;
            //_mapping = ASPMapperHelper.AspFormat(configuration.ConfigurationName) + "(" + ASPMapperHelper.AspFormat(gameObject.name) + ",objectIndex(" + index + ")," + mapping + ")." + Environment.NewLine;
            _mapping = ASPMapperHelper.AspFormat(features.PropertyAlias) + "(" + ASPMapperHelper.AspFormat(gameObject.name) + ",objectIndex(" + index + ")," + mapping + ")." + Environment.NewLine;
            invariant = information.invariant;
            ready = true;
        }

        /*
        internal void UpdateValue()
        {
            if (!ready)
            {
                return;
            }
            if (!invariant || first)
            {
                first = false;
                MapperManager.UpdateSensor(this, gameObject, new MyListString(property.myStrings), 0);
            }
        }
        */
        /*
        internal string Map()
        {
            //VELOCIZZA EVITANDO COPIE!!
            List<object> values = new List<object>();
            for (int i = 0; i < PropertyInfo.Count; i++)
            {
                object toAdd = PropertyInfo[i].GetValuesForPlaceholders();
                if (toAdd != null)
                {
                    if (toAdd is IList listToAdd)
                    {
                        for (int j = 0; j < listToAdd.Count; j++)
                        {
                            values.Add(listToAdd[j]);
                        }
                    }
                    else
                    {
                        values.Add(toAdd);
                    }
                }
            }
            //return MapperManager.GetSensorBasicMap(this, gameObject, new MyListString(property.myStrings), new List<object>(), 0);
            try
            {
                return string.Format(Mapping, values.ToArray());
            }
            catch (FormatException e)
            {
                return "";
            }
        }*/

        /*
        internal void Destroy()
        {
            manager.Destroy(this);
        }*/


        //GMDG
        protected string mappingTemplate = string.Empty;

        public abstract void Initialize(SensorConfiguration sensorConfiguration);

        public abstract void Destroy();

        public abstract void Update();

        public abstract string Map();
        //GMDG
    }
}