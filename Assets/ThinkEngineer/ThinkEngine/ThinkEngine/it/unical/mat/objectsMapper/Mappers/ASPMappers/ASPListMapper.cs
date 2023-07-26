using ThinkEngine.Mappers.BaseMappers;
using System;
using System.Collections;
using System.Collections.Generic;
using ThinkEngine.Mappers.BaseMappers;
using UnityEngine;

namespace ThinkEngine.Mappers.ASPMappers
{
    class ASPListMapper : CollectionMapper
    {
        #region SUPPORT CLASSES

        private class ListSensors : CollectionOfSensors
        {
            internal List<ISensors> sensorsList;
            internal override IEnumerable sensorsCollection {
                get
                {
                    return sensorsList;
                }
            }

            internal ListSensors()
            {
                sensorsList = new List<ISensors>();
            }

        }
        private class ListActuators : CollectionOfActuators
        {
            internal List<IActuators> actuatorsList;

            internal override IEnumerable actuatorsCollection {
                get
                {
                    return actuatorsList;
                }
            }
            internal ListActuators()
            {
                actuatorsList = new List<IActuators>();
            }

        }
        #endregion
        private static ASPListMapper _instance;
        internal static ASPListMapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ASPListMapper();
                }
                return _instance;
            }
        }
        public override Type ElementType(Type type)
        {
            return type.GenericTypeArguments[0];
        }
        public override bool Supports(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
        public override ISensors InstantiateSensors(InstantiationInformation information)
        {
            if (information.currentObjectOfTheHierarchy == null)
            {
                return null;
            }
            IList actualList = (IList)information.currentObjectOfTheHierarchy;
            int x = actualList.Count;
            ListSensors sensors = new ListSensors();
            if (x == 0)
            {
                return sensors;
            }
            if (actualList[0] == null)
            {
                return sensors;
            }
            GenerateMapping(ref information, actualList[0].GetType());
            information.firstPlaceholder++;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, actualList[0].GetType());
            for (int i = 0; i < x; i++)
            {
                sensors.sensorsList.Add(InstantiateSensorsForElement(information, actualList, i));
                if (sensors.sensorsList[i] == null)
                {
                    sensors.needsUpdate = true;
                }
            }
            return sensors;

        }
        private ISensors GenerateUpdateSensors(InstantiationInformation information, ListSensors sensors, IList actualList, int x)
        {
            ListSensors toReturn = new ListSensors();
            if (x == 0)
            {
                return toReturn;
            }
            if (actualList[0] == null)
            {
                return toReturn;
            }
            Type elementType = actualList[0].GetType();
            if (!information.mappingDone)
            {
                GenerateMapping(ref information, elementType);
            }
            information.firstPlaceholder += 1;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            {
                if (sensors.sensorsList.Count > i && sensors.sensorsList[i]!=null)
                {
                    toReturn.sensorsList.Add(sensors.sensorsList[i]);
                }
                else
                {
                    toReturn.sensorsList.Add(InstantiateSensorsForElement(information, actualList, i));
                    if (toReturn.sensorsList[i] == null)
                    {
                        sensors.needsUpdate = true;
                    }
                }
            }
            return toReturn;
        }
        public override ISensors ManageSensors(InstantiationInformation information, ISensors instantiatedSensors)
        {
            if (instantiatedSensors == null)
            {
                return InstantiateSensors(information);
            }
            if (!(instantiatedSensors is ListSensors))
            {
                Debug.LogError("Error in sensors generation.");
            }
            ListSensors sensors = (ListSensors)instantiatedSensors;
            if (sensors.sensorsList.Count == 0)
            {
                return InstantiateSensors(information);
            }
            if (information.currentObjectOfTheHierarchy == null && sensors.sensorsList.Count > 0)
            {
                return new ListSensors();
            }
            IList actualList = (IList)information.currentObjectOfTheHierarchy;
            int x = actualList.Count;
            if (!sensors.needsUpdate && x == sensors.sensorsList.Count)
            {
                return instantiatedSensors;
            }
            return GenerateUpdateSensors(information, sensors, actualList, x);
        }
        public override void UpdateSensor(Sensor sensor, object currentObject, MyListString residualPropertyHierarchy, int hierarchyLevel)
        {
            IList list = (IList)currentObject;
            CollectionInfoAndValue info = (CollectionInfoAndValue)sensor.PropertyInfo[hierarchyLevel];
            if (list.Count > info.indexes[0])
            {
                UpdateResidualPropertyHierarchy(residualPropertyHierarchy, list[info.indexes[0]].GetType());
                MapperManager.UpdateSensor(sensor, list[info.indexes[0]], residualPropertyHierarchy, hierarchyLevel + 1);
            }
            else
            {
                sensor.Destroy();
            }
        }
        public override IActuators InstantiateActuators(InstantiationInformation information)
        {
            if (information.currentObjectOfTheHierarchy == null)
            {
                return new ListActuators();
            }
            IList actualList = (IList)information.currentObjectOfTheHierarchy;
            int x = actualList.Count;
            ListActuators actuators = new ListActuators();
            if (x == 0)
            {
                return actuators;
            }
            if (actualList[0] == null)
            {
                return actuators;
            }
            GenerateMapping(ref information, actualList[0].GetType());
            information.firstPlaceholder += 1;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, actualList[0].GetType());
            for (int i = 0; i < x; i++)
            {
                actuators.actuatorsList.Add(InstantiateActuatorsForElement(information, actualList, i));
                if (actuators.actuatorsList[i] == null)
                {
                    actuators.needsUpdates = true;
                }
            }
            return actuators;
        }
        public override IActuators ManageActuators(InstantiationInformation information, IActuators instantiatedActuators)
        {
            if (instantiatedActuators == null)
            {
                return InstantiateActuators(information);
            }
            if (!(instantiatedActuators is ListActuators))
            {
                Debug.LogError("Error in sensors generation.");
            }
            ListActuators actuators = (ListActuators)instantiatedActuators;
            if (actuators.actuatorsList.Count == 0)
            {
                return InstantiateActuators(information);
            }
            if (information.currentObjectOfTheHierarchy == null && actuators.actuatorsList.Count > 0)
            {
                return new ListActuators();
            }
            IList actualList = (IList)information.currentObjectOfTheHierarchy;
            int x = actualList.Count;
            if (x == 0)
            {
                return new ListActuators();
            }
            if (!actuators.needsUpdates && x == actuators.actuatorsList.Count)
            {
                return instantiatedActuators;
            }
            return GenerateUpdateActuators(information, actuators, actualList, x);
        }
        private IActuators GenerateUpdateActuators(InstantiationInformation information, ListActuators actuators, IList actualList, int x)
        {
            ListActuators toReturn = new ListActuators();
            if (x == 0)
            {
                return toReturn;
            }
            if (actualList[0] == null)
            {
                return toReturn;
            }
            Type elementType = actualList[0].GetType();
            if (!information.mappingDone)
            {
                GenerateMapping(ref information, elementType);
            }
            information.firstPlaceholder += 1;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            { 
                if (actuators.actuatorsList.Count > i && actuators.actuatorsList[i] != null)
                {
                    toReturn.actuatorsList.Add(actuators.actuatorsList[i]);
                }
                else
                {
                    toReturn.actuatorsList.Add(InstantiateActuatorsForElement(information, actualList, i));
                    if (toReturn.actuatorsList[i] == null)
                    {
                        actuators.needsUpdates = true;
                    }
                }
            }
            return toReturn;
        }
        public override void SetPropertyValue(MonoBehaviourActuator actuator, MyListString residualPropertyHierarchy, object currentObject, object valueToSet, int hierarchyLevel)
        {
            IList list = (IList)currentObject;
            CollectionInfoAndValue info = (CollectionInfoAndValue)actuator.PropertyInfo[hierarchyLevel];
            if (list.Count > info.indexes[0])
            {
                currentObject = list[info.indexes[0]];
                Type elementType = currentObject.GetType();
                UpdateResidualPropertyHierarchy(residualPropertyHierarchy, elementType);
                if (MapperManager.IsBasic(elementType))
                {
                    list[info.indexes[0]]= Convert.ChangeType(valueToSet, elementType);
                    return;
                }
                MapperManager.SetPropertyValue(actuator, residualPropertyHierarchy, currentObject, valueToSet, hierarchyLevel + 1);
            }
            else
            {
                UnityEngine.Object.Destroy(actuator);
            }
        }

        protected override string Placeholders(InstantiationInformation information)
        {
            return "{" + information.firstPlaceholder + "},";
        }
        protected override void AddVariables(InstantiationInformation information, List<string> variables)
        {
            variables.Add("Index" + (information.firstPlaceholder + 1));
        }
        protected override void IncreasePlaceholders(InstantiationInformation information)
        {
            information.firstPlaceholder++;
        }
        protected override void UpdateCurrentObjectOfTheHierarchy(InstantiationInformation information)
        {
            IList list = (IList)information.currentObjectOfTheHierarchy;
            if (list.Count > 0)
            {
                information.currentObjectOfTheHierarchy = list[0];
            }
            else
            {
                information.currentObjectOfTheHierarchy = null;
            }
        }
        protected override object ElementOfTheCollection(object actualCollection, params int[] indexes)
        {
            return ((IList)actualCollection)[indexes[0]];
        }
    }
}
