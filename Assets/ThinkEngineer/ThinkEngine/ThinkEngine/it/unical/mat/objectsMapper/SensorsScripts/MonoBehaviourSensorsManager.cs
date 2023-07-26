using ThinkEngine.Mappers;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ThinkEngine
{
    class MonoBehaviourSensorsManager : MonoBehaviour
    {
        List<MyListString> invariantSensors;
        private Thread mainThread;
        Dictionary<MyListString, int> propertiesIndex;
        Dictionary<int, InstantiationInformation> instantiationInformationForProperty;
        Dictionary<int, ISensors> monoBehaviourSensorsForProperty;
        internal bool ready;
        private int frameToWait;
        SensorConfiguration[] configurations;
        internal Dictionary<SensorConfiguration, List<Sensor>> _sensorsForConfigurations;
        internal Dictionary<SensorConfiguration, List<Sensor>> Sensors
        {
            get
            {
                if (_sensorsForConfigurations == null)
                {
                    _sensorsForConfigurations = new Dictionary<SensorConfiguration, List<Sensor>>();
                }
                return _sensorsForConfigurations;
            }
        }

        void Start()
        {
            invariantSensors = new List<MyListString>();
            mainThread = Thread.CurrentThread;
            propertiesIndex = new Dictionary<MyListString, int>();
            instantiationInformationForProperty = new Dictionary<int, InstantiationInformation>();
            monoBehaviourSensorsForProperty = new Dictionary<int, ISensors>();
            foreach (SensorConfiguration configuration in GetComponents<SensorConfiguration>())
            {
                Sensors[configuration] = new List<Sensor>();
                foreach (MyListString property in configuration.ToMapProperties)
                {
                    if (configuration.isInvariant || configuration.isFixedSize)
                    {
                        invariantSensors.Add(property);
                    }
                    int propertyIndex = property.GetHashCode();
                    propertiesIndex[property] = propertyIndex;
                    InstantiationInformation information = new InstantiationInformation()
                    {
                        instantiateOn = gameObject,
                        currentObjectOfTheHierarchy = gameObject,
                        propertyHierarchy = new MyListString(property.myStrings),
                        residualPropertyHierarchy = new MyListString(property.myStrings),
                        firstPlaceholder = 0,
                        configuration = configuration,
                        invariant = configuration.isInvariant
                    };
                    instantiationInformationForProperty[propertyIndex] = information;
                    monoBehaviourSensorsForProperty[propertyIndex] = MapperManager.InstantiateSensors(information);
                    if (monoBehaviourSensorsForProperty[propertyIndex] != null)
                    {
                        Sensors[configuration].AddRange(monoBehaviourSensorsForProperty[propertyIndex].GetSensorsList());
                    }
                }
            }
            ready = true;
        }

        internal void Destroy(Sensor monoBehaviourSensor)
        {
            if (_sensorsForConfigurations.ContainsKey(monoBehaviourSensor.configuration))
            {
                _sensorsForConfigurations[monoBehaviourSensor.configuration].Remove(monoBehaviourSensor);
            }
        }

        internal void ManageSensors()
        {
            configurations = GetComponents<SensorConfiguration>();

            foreach (SensorConfiguration configuration in Sensors.Keys)
            {
                if (configuration.isFixedSize || configuration.isInvariant)
                {
                    continue;
                }
                Sensors[configuration].Clear();
            }
            foreach (MyListString property in propertiesIndex.Keys)
            {
                if (invariantSensors.Contains(property))
                {
                    continue;
                }
                int propertyIndex = propertiesIndex[property];
                ISensors sensors = monoBehaviourSensorsForProperty[propertyIndex];
                InformationRefresh(propertyIndex);
                monoBehaviourSensorsForProperty[propertyIndex] = MapperManager.ManageSensors(instantiationInformationForProperty[propertyIndex], sensors);
                if (monoBehaviourSensorsForProperty[propertyIndex] != null)
                {
                    Sensors[(SensorConfiguration)instantiationInformationForProperty[propertyIndex].configuration].AddRange(monoBehaviourSensorsForProperty[propertyIndex].GetSensorsList());
                }
            }
        }
        /*void LateUpdate()
        {
            if (!ready)
            {
                return;
            }
            if (SensorsManager.frameFromLastUpdate >= SensorsManager.updateFrequencyInFrames+frameToWait)
            {
                MyLateUpdate();
                foreach (List<MonoBehaviourSensor> sensorsList in Sensors.Values)
                {
                    foreach (MonoBehaviourSensor sensor in sensorsList)
                    {
                        sensor.MyLateUpdate();
                    }
                }
            }
        }*/
        private void InformationRefresh(int propertyIndex)
        {
            instantiationInformationForProperty[propertyIndex].currentObjectOfTheHierarchy = gameObject;
            instantiationInformationForProperty[propertyIndex].residualPropertyHierarchy = new MyListString(instantiationInformationForProperty[propertyIndex].propertyHierarchy.myStrings);
            instantiationInformationForProperty[propertyIndex].firstPlaceholder = 0;
        }

        internal bool ExistsConfigurationOtherThan(string name, SensorConfiguration newSensorConfiguration)
        {
            foreach (SensorConfiguration configuration in GetComponents<SensorConfiguration>())
            {
                if (configuration != newSensorConfiguration && configuration.ConfigurationName.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        internal SensorConfiguration GetConfiguration(string confName)
        {
            if (mainThread == null || mainThread.Equals(Thread.CurrentThread))
            {
                configurations = GetComponents<SensorConfiguration>();
            }
            if (configurations == null || configurations.Length == 0)
            {
                if (Application.isPlaying)
                {
                    Destroy(this);
                }
                else
                {
                    DestroyImmediate(this);
                }
                return null;
            }
            foreach (SensorConfiguration configuration in configurations)
            {
                if (configuration.ConfigurationName.Equals(confName))
                {
                    return configuration;
                }
            }
            return null;
        }

        internal IEnumerable<string> GetAllConfigurationNames()
        {
            List<string> toReturn = new List<string>();
            if (mainThread == null || mainThread.Equals(Thread.CurrentThread))
            {
                configurations = GetComponents<SensorConfiguration>();
            }
            if (configurations == null || configurations.Length == 0)
            {
                if (Application.isPlaying)
                {
                    Destroy(this);
                }
                else
                {
                    DestroyImmediate(this);
                }
                return toReturn;
            }
            foreach (SensorConfiguration configuration in configurations)
            {
                toReturn.Add(configuration.ConfigurationName);
            }
            return toReturn;
        }
    }
}