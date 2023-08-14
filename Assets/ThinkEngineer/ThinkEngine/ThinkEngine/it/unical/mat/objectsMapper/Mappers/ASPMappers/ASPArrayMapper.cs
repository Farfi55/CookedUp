using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkEngine.Mappers.BaseMappers;
using UnityEngine;

namespace ThinkEngine.Mappers.BaseMappers
{
    class ASPArrayMapper : CollectionMapper
    {
        #region SUPPORT CLASSES

        private class ArraySensors : CollectionOfSensors
        {
            internal ISensors[] sensorsArray;
            internal override IEnumerable sensorsCollection
            {
                get
                {
                    return sensorsArray;
                }
            }
            internal ArraySensors(int i)
            {
                sensorsArray = new ISensors[i];
            }
        }
        private class ArrayActuators : CollectionOfActuators
        {
            internal IActuators[] actuatorsArray;
            internal override IEnumerable actuatorsCollection
            {
                get
                {
                    return actuatorsArray;
                }
            }
            internal ArrayActuators(int i)
            {
                actuatorsArray = new IActuators[i];
            }

        }
        #endregion
        private static ASPArrayMapper _instance;
        internal static ASPArrayMapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ASPArrayMapper();
                }
                return _instance;
            }
        }
        public override Type ElementType(Type type)
        {
            return type.GetElementType();
        }

        public override bool Supports(Type type)
        {
            return type.IsArray && type.GetArrayRank() == 1;
        }
        public override ISensors InstantiateSensors(InstantiationInformation information)
        {
            if (information.currentObjectOfTheHierarchy == null)
            {
                return null;
            }
            Array actualArray = (Array)information.currentObjectOfTheHierarchy;
            int x = actualArray.GetLength(0);
            ArraySensors sensors = new ArraySensors(x);
            if (x == 0)
            {
                return sensors;
            }
            if (actualArray.GetValue(0) == null)
            {
                return sensors;
            }
            Type elementType = actualArray.GetValue(0).GetType();
            GenerateMapping(ref information, elementType);
            information.firstPlaceholder += 1;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            {
                sensors.sensorsArray[i] = InstantiateSensorsForElement(information, actualArray, i);
                if (sensors.sensorsArray[i] == null)
                {
                    sensors.needsUpdate = true;
                }
            }
            return sensors;
        }
        private ISensors GenerateUpdateSensors(InstantiationInformation information, ArraySensors sensors, Array actualArray, int x)
        {
            ArraySensors toReturn = new ArraySensors(x);
            if (x == 0)
            {
                return toReturn;
            }
            if (actualArray.GetValue(0) == null)
            {
                return toReturn;
            }
            Type elementType = actualArray.GetValue(0).GetType();
            if (!information.mappingDone)
            {
                GenerateMapping(ref information, elementType);
            }
            information.firstPlaceholder += 1;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            {
                if (sensors.sensorsArray.GetLength(0) > i && sensors.sensorsArray[i] != null)
                {

                    toReturn.sensorsArray[i] = sensors.sensorsArray[i];
                }
                else
                {
                    toReturn.sensorsArray[i] = InstantiateSensorsForElement(information, actualArray, i);
                    if (toReturn.sensorsArray[i] == null)
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
            if (!(instantiatedSensors is ArraySensors))
            {
                Debug.LogError("Error in sensors generation.");
            }
            ArraySensors sensors = (ArraySensors)instantiatedSensors;
            if (sensors.sensorsArray.Length == 0)
            {
                return InstantiateSensors(information);
            }
            if (information.currentObjectOfTheHierarchy == null && sensors.sensorsArray.Length > 0)
            {
                return new ArraySensors(0);
            }
            Array actualArray = (Array)information.currentObjectOfTheHierarchy;
            int x = actualArray.GetLength(0);
            if (!sensors.needsUpdate && x == sensors.sensorsArray.GetLength(0))
            {
                return instantiatedSensors;
            }
            return GenerateUpdateSensors(information, sensors, actualArray, x);
        }
        public override void UpdateSensor(Sensor sensor, object currentObject, MyListString residualPropertyHierarchy, int hierarchyLevel)
        {
            Array array = (Array)currentObject;
            CollectionInfoAndValue info = (CollectionInfoAndValue)sensor.PropertyInfo[hierarchyLevel];
            if (array.GetLength(0) > info.indexes[0])
            {
                object arrayElement = array.GetValue(info.indexes[0]);
                UpdateResidualPropertyHierarchy(residualPropertyHierarchy, arrayElement.GetType());
                MapperManager.UpdateSensor(sensor, arrayElement, residualPropertyHierarchy, hierarchyLevel + 1);
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
                return new ArrayActuators(0);
            }
            Array actualArray = (Array)information.currentObjectOfTheHierarchy;
            int x = actualArray.GetLength(0);
            ArrayActuators actuators = new ArrayActuators(x);
            if (x == 0)
            {
                return actuators;
            }
            if (actualArray.GetValue(0) == null)
            {
                return actuators;
            }
            Type elementType = actualArray.GetValue(0).GetType();
            GenerateMapping(ref information, elementType);
            information.firstPlaceholder += 1;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            {
                actuators.actuatorsArray[i] = InstantiateActuatorsForElement(information, actualArray, i);
                if (actuators.actuatorsArray[i] == null)
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
            if (!(instantiatedActuators is ArrayActuators))
            {
                Debug.LogError("Error in sensors generation.");
            }
            ArrayActuators actuators = (ArrayActuators)instantiatedActuators;
            if (actuators.actuatorsArray.Length == 0)
            {
                return InstantiateActuators(information);
            }
            if (information.currentObjectOfTheHierarchy == null && actuators.actuatorsArray.Length > 0)
            {
                return new ArrayActuators(0);
            }
            Array actualArray = (Array)information.currentObjectOfTheHierarchy;
            int x = actualArray.GetLength(0);
            if (!actuators.needsUpdates && x == actuators.actuatorsArray.GetLength(0))
            {
                return instantiatedActuators;
            }
            return GenerateUpdateActuators(information, actuators, actualArray, x);
        }
        private IActuators GenerateUpdateActuators(InstantiationInformation information, ArrayActuators actuators, Array actualArray, int x)
        {
            ArrayActuators toReturn = new ArrayActuators(x);
            if (x == 0)
            {
                return toReturn;
            }
            if (actualArray.GetValue(0) == null)
            {
                return actuators;
            }
            Type elementType = actualArray.GetValue(0).GetType();
            if (!information.mappingDone)
            {
                GenerateMapping(ref information, elementType);
            }
            information.firstPlaceholder += 1;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            {
                if (actuators.actuatorsArray.GetLength(0) > i && actuators.actuatorsArray.GetValue(i) != null)
                {
                    toReturn.actuatorsArray[i] = actuators.actuatorsArray[i];
                }
                else
                {
                    toReturn.actuatorsArray[i] = InstantiateActuatorsForElement(information, actualArray, i);
                    if (toReturn.actuatorsArray.GetValue(i) == null)
                    {
                        actuators.needsUpdates = true;
                    }
                }
            }
            return toReturn;
        }
        public override void SetPropertyValue(MonoBehaviourActuator actuator, MyListString residualPropertyHierarchy, object currentObject, object valueToSet, int hierarchyLevel)
        {
            Array aray = (Array)currentObject;
            CollectionInfoAndValue info = (CollectionInfoAndValue)actuator.PropertyInfo[hierarchyLevel];
            if (aray.GetLength(0) > info.indexes[0])
            {
                currentObject = aray.GetValue(info.indexes[0]);
                Type elementType = currentObject.GetType();
                UpdateResidualPropertyHierarchy(residualPropertyHierarchy, elementType);
                if (MapperManager.IsBasic(elementType))
                {
                    aray.SetValue(Convert.ChangeType(valueToSet, elementType), info.indexes[0]);
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
            information.firstPlaceholder += 1;
        }
        protected override void UpdateCurrentObjectOfTheHierarchy(InstantiationInformation information)
        {
            Array matrix = (Array)information.currentObjectOfTheHierarchy;
            if (matrix.Length > 0)
            {
                information.currentObjectOfTheHierarchy = matrix.GetValue(0);
            }
            else
            {
                information.currentObjectOfTheHierarchy = null;
            }
        }
        protected override object ElementOfTheCollection(object actualCollection, params int[] indexes)
        {
            return ((Array)actualCollection).GetValue(indexes);
        }

    }
}
