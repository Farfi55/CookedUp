using System;
using System.Collections;
using System.Collections.Generic;
using ThinkEngine.Mappers.BaseMappers;
using UnityEngine;

namespace ThinkEngine.Mappers.BaseMappers
{
    class ASPArray2Mapper : CollectionMapper
    {
        #region SUPPORT CLASSES
       
        private class Array2Sensors : CollectionOfSensors
        {
            internal ISensors[,] sensorsMatrix;
            internal override IEnumerable sensorsCollection
            {
                get
                {
                    return sensorsMatrix;
                }
            }
            internal Array2Sensors(int i, int j)
            {
                sensorsMatrix = new ISensors[i, j];
            }
        }
        private class Array2Actuators : CollectionOfActuators
        {
            internal IActuators[,] actuatorsMatrix;
            internal override IEnumerable actuatorsCollection
            {
                get
                {
                    return actuatorsMatrix;
                }
            }
            internal Array2Actuators(int i, int j)
            {
                actuatorsMatrix = new IActuators[i, j];
            }

        }
        #endregion
        private static ASPArray2Mapper _instance;
        internal static ASPArray2Mapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ASPArray2Mapper();
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
            return type.IsArray && type.GetArrayRank() == 2;
        }
        public override ISensors InstantiateSensors(InstantiationInformation information)
        {
            if (information.currentObjectOfTheHierarchy == null)
            {
                return null;
            }
            Array actualMatrix = (Array)information.currentObjectOfTheHierarchy;
            int x = actualMatrix.GetLength(0);
            int y = actualMatrix.GetLength(1);
            Array2Sensors sensors = new Array2Sensors(x, y);
            if (x == 0 || y==0)
            {
                return sensors;
            }
            if(actualMatrix.GetValue(0, 0) == null)
            {
                return sensors;
            }
            Type elementType = actualMatrix.GetValue(0, 0).GetType();
            GenerateMapping(ref information, elementType);
            information.firstPlaceholder += 2;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    sensors.sensorsMatrix[i, j]=InstantiateSensorsForElement(information, actualMatrix, i, j);
                    if (sensors.sensorsMatrix[i, j] == null)
                    {
                        sensors.needsUpdate=true;
                    }
                }
            }
            return sensors;
        }
        private ISensors GenerateUpdateSensors(InstantiationInformation information, Array2Sensors sensors, Array actualMatrix, int x, int y)
        {
            Array2Sensors toReturn = new Array2Sensors(x, y);
            if (x == 0 || y==0)
            {
                return toReturn;
            }
            if (actualMatrix.GetValue(0, 0) == null)
            {
                return toReturn;
            }
            Type elementType = actualMatrix.GetValue(0, 0).GetType();
            if (!information.mappingDone)
            {
                GenerateMapping(ref information, elementType);
            }
            information.firstPlaceholder += 2;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (sensors.sensorsMatrix.GetLength(0) > i && sensors.sensorsMatrix.GetLength(1) > j && sensors.sensorsMatrix[i, j] != null)
                    {
                        
                        toReturn.sensorsMatrix[i, j] = sensors.sensorsMatrix[i, j];
                    }
                    else
                    {
                        toReturn.sensorsMatrix[i, j] = InstantiateSensorsForElement(information, actualMatrix, i, j);
                        if (toReturn.sensorsMatrix[i, j] == null)
                        {
                            sensors.needsUpdate = true;
                        }
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
            if (!(instantiatedSensors is Array2Sensors))
            {
                Debug.LogError("Error in sensors generation.");
            }
            Array2Sensors sensors = (Array2Sensors)instantiatedSensors;
            if (sensors.sensorsMatrix.Length == 0)
            {
                return InstantiateSensors(information);
            }
            if (information.currentObjectOfTheHierarchy == null && sensors.sensorsMatrix.Length > 0)
            {
                return new Array2Sensors(0, 0);
            }
            Array actualMatrix = (Array)information.currentObjectOfTheHierarchy;
            int x = actualMatrix.GetLength(0);
            int y = actualMatrix.GetLength(1);
            if (!sensors.needsUpdate && x == sensors.sensorsMatrix.GetLength(0) && y == sensors.sensorsMatrix.GetLength(1))
            {
                return instantiatedSensors;
            }
            return GenerateUpdateSensors(information, sensors, actualMatrix, x, y);
        }
        public override void UpdateSensor(Sensor sensor, object currentObject, MyListString residualPropertyHierarchy, int hierarchyLevel)
        {
            Array matrix = (Array)currentObject;
            CollectionInfoAndValue info = (CollectionInfoAndValue)sensor.PropertyInfo[hierarchyLevel];
            if (matrix.GetLength(0) > info.indexes[0] && matrix.GetLength(1) > info.indexes[1])
            {
                object matrixElement = matrix.GetValue(info.indexes[0], info.indexes[1]);
                UpdateResidualPropertyHierarchy(residualPropertyHierarchy, matrixElement.GetType());
                MapperManager.UpdateSensor(sensor, matrixElement, residualPropertyHierarchy, hierarchyLevel + 1);
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
                return new Array2Actuators(0, 0);
            }
            Array actualMatrix = (Array)information.currentObjectOfTheHierarchy;
            int x = actualMatrix.GetLength(0);
            int y = actualMatrix.GetLength(1);
            Array2Actuators actuators = new Array2Actuators(x, y);
            if (x == 0 || y==0)
            {
                return actuators;
            }
            if (actualMatrix.GetValue(0, 0) == null)
            {
                return actuators;
            }
            Type elementType = actualMatrix.GetValue(0, 0).GetType();
            GenerateMapping(ref information,elementType);
            information.firstPlaceholder += 2;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    actuators.actuatorsMatrix[i, j] = InstantiateActuatorsForElement(information, actualMatrix, i, j);
                    if (actuators.actuatorsMatrix[i,j] == null)
                    {
                        actuators.needsUpdates = true;
                    }
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
            if (!(instantiatedActuators is Array2Actuators))
            {
                Debug.LogError("Error in sensors generation.");
            }
            Array2Actuators actuators = (Array2Actuators)instantiatedActuators;
            if (actuators.actuatorsMatrix.Length == 0)
            {
                return InstantiateActuators(information);
            }
            if (information.currentObjectOfTheHierarchy == null && actuators.actuatorsMatrix.Length > 0)
            {
                return new Array2Actuators(0, 0);
            }
            Array actualMatrix = (Array)information.currentObjectOfTheHierarchy;
            int x = actualMatrix.GetLength(0);
            int y = actualMatrix.GetLength(1);
            if (!actuators.needsUpdates && x == actuators.actuatorsMatrix.GetLength(0) && y == actuators.actuatorsMatrix.GetLength(1))
            {
                return instantiatedActuators;
            }
            return GenerateUpdateActuators(information, actuators, actualMatrix, x, y);
        }
        private IActuators GenerateUpdateActuators(InstantiationInformation information, Array2Actuators actuators, Array actualMatrix, int x, int y)
        {
            Array2Actuators toReturn = new Array2Actuators(x, y);
            if (x == 0 || y==0)
            {
                return toReturn;
            }
            if (actualMatrix.GetValue(0, 0) == null)
            {
                return actuators;
            }
            Type elementType = actualMatrix.GetValue(0,0).GetType();
            if (!information.mappingDone)
            {
                GenerateMapping(ref information, elementType);
            }
            information.firstPlaceholder += 2;
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, elementType);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (actuators.actuatorsMatrix.GetLength(0) > i && actuators.actuatorsMatrix.GetLength(1) > j  && actuators.actuatorsMatrix.GetValue(i,j) != null)
                    {
                        toReturn.actuatorsMatrix[i, j] = actuators.actuatorsMatrix[i, j];
                    }
                    else
                    {
                        toReturn.actuatorsMatrix[i, j] = InstantiateActuatorsForElement(information, actualMatrix, i, j);
                        if (toReturn.actuatorsMatrix.GetValue(i,j) == null)
                        {
                            actuators.needsUpdates = true;
                        }
                    }
                }
            }
            return toReturn;
        }
        public override void SetPropertyValue(MonoBehaviourActuator actuator, MyListString residualPropertyHierarchy,  object currentObject, object valueToSet, int hierarchyLevel)
        {
            Array matrix = (Array)currentObject;
            CollectionInfoAndValue info = (CollectionInfoAndValue)actuator.PropertyInfo[hierarchyLevel];
            if (matrix.GetLength(0) > info.indexes[0] && matrix.GetLength(1) > info.indexes[1])
            {
                currentObject = matrix.GetValue(info.indexes[0], info.indexes[1]);
                Type elementType = currentObject.GetType();
                UpdateResidualPropertyHierarchy(residualPropertyHierarchy, elementType);
                if (MapperManager.IsBasic(elementType))
                {
                    matrix.SetValue(Convert.ChangeType(valueToSet, elementType), info.indexes[0], info.indexes[1]);
                    return;
                }
                MapperManager.SetPropertyValue(actuator, residualPropertyHierarchy,  currentObject, valueToSet, hierarchyLevel + 1);
            }
            else
            {
                UnityEngine.Object.Destroy(actuator);
            }
        }

        protected override string Placeholders(InstantiationInformation information)
        {
            return "{" + information.firstPlaceholder + "},{" + (information.firstPlaceholder + 1) + "},";
        }
        protected override void AddVariables(InstantiationInformation information, List<string> variables)
        {
            variables.Add("Index" + (information.firstPlaceholder + 1));
            variables.Add("Index" + (information.firstPlaceholder + 2));
        }
        protected override void IncreasePlaceholders(InstantiationInformation information)
        {
            information.firstPlaceholder+=2;
        }
        protected override void UpdateCurrentObjectOfTheHierarchy(InstantiationInformation information)
        {
            Array matrix = (Array)information.currentObjectOfTheHierarchy;
            if (matrix.Length > 0)
            {
                information.currentObjectOfTheHierarchy = matrix.GetValue(0, 0);
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
