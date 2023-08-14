using System.Collections.Generic;
using ThinkEngine.Mappers;
using UnityEngine;

namespace ThinkEngine
{
    internal class MonoBehaviourActuatorsManager : MonoBehaviour
    {
        Dictionary<MyListString, int> propertiesIndex;
        Dictionary<int, InstantiationInformation> instantiationInformationForProperty;
        Dictionary<int, IActuators> monoBehaviourActuatorsForProperty;
        internal bool ready;
        internal Dictionary<ActuatorConfiguration, List<MonoBehaviourActuator>> _actuators;
        internal Dictionary<ActuatorConfiguration, List<MonoBehaviourActuator>> Actuators
        {
            get
            {
                if (_actuators == null)
                {
                    _actuators = new Dictionary<ActuatorConfiguration, List<MonoBehaviourActuator>>();
                }
                return _actuators;
            }
        }

        void Start()
        {
            propertiesIndex = new Dictionary<MyListString, int>();
            instantiationInformationForProperty = new Dictionary<int, InstantiationInformation>();
            monoBehaviourActuatorsForProperty = new Dictionary<int, IActuators>();
            foreach (ActuatorConfiguration configuration in GetComponents<ActuatorConfiguration>())
            {
                Actuators[configuration] = new List<MonoBehaviourActuator>();
                foreach (MyListString property in configuration.ToMapProperties)
                {
                    int propertyIndex = property.GetHashCode();
                    propertiesIndex[property] = propertyIndex;
                    InstantiationInformation information = new InstantiationInformation()
                    {
                        instantiateOn = gameObject,
                        currentObjectOfTheHierarchy = gameObject,
                        propertyHierarchy = new MyListString(property.myStrings),
                        residualPropertyHierarchy = new MyListString(property.myStrings),
                        firstPlaceholder = 0,
                        configuration = configuration
                    };
                    instantiationInformationForProperty[propertyIndex] = information;
                    monoBehaviourActuatorsForProperty[propertyIndex] = MapperManager.InstantiateActuators(information);
                    if (monoBehaviourActuatorsForProperty[propertyIndex] != null)
                    {
                        Actuators[configuration].AddRange(monoBehaviourActuatorsForProperty[propertyIndex].GetActuatorsList());
                    }
                }
            }
            ready = true;
        }
        void Update()
        {
            foreach (ActuatorConfiguration configuration in Actuators.Keys)
            {
                Actuators[configuration].Clear();
            }
            foreach (MyListString property in propertiesIndex.Keys)
            {
                int propertyIndex = propertiesIndex[property];
                IActuators actuators = monoBehaviourActuatorsForProperty[propertyIndex];
                InformationRefresh(propertyIndex);
                monoBehaviourActuatorsForProperty[propertyIndex] = MapperManager.ManageActuators(instantiationInformationForProperty[propertyIndex], actuators);
                if (monoBehaviourActuatorsForProperty[propertyIndex] != null)
                {
                    Actuators[(ActuatorConfiguration)instantiationInformationForProperty[propertyIndex].configuration].AddRange(monoBehaviourActuatorsForProperty[propertyIndex].GetActuatorsList());
                }
            }
        }

        private void InformationRefresh(int propertyIndex)
        {
            instantiationInformationForProperty[propertyIndex].currentObjectOfTheHierarchy = gameObject;
            instantiationInformationForProperty[propertyIndex].residualPropertyHierarchy = new MyListString(instantiationInformationForProperty[propertyIndex].propertyHierarchy.myStrings);
            instantiationInformationForProperty[propertyIndex].firstPlaceholder = 0;
        }
        internal bool ExistsConfigurationOtherThan(string name, ActuatorConfiguration newActuatorConfiguration)
        {
            foreach (ActuatorConfiguration configuration in GetComponents<ActuatorConfiguration>())
            {
                if (configuration != newActuatorConfiguration && configuration.ConfigurationName.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        internal IEnumerable<string> GetAllConfigurationNames()
        {
            List<string> toReturn = new List<string>();
            ActuatorConfiguration[] configurations = GetComponents<ActuatorConfiguration>();
            if (configurations == null || configurations.Length == 0)
            {
                Destroy(this);
                return toReturn;
            }
            foreach (ActuatorConfiguration configuration in configurations)
            {
                toReturn.Add(configuration.ConfigurationName);
            }
            return toReturn;
        }

        internal ActuatorConfiguration GetConfiguration(string confName)
        {
            ActuatorConfiguration[] configurations = GetComponents<ActuatorConfiguration>();
            if (configurations == null || configurations.Length == 0)
            {
                Destroy(this);
                return null;
            }
            foreach (ActuatorConfiguration configuration in configurations)
            {
                if (configuration.ConfigurationName.Equals(confName))
                {
                    return configuration;
                }
            }
            return null;
        }
    }
}