using ThinkEngine.Mappers.BaseMappers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThinkEngine.Mappers.BaseMappers
{
    internal abstract class CollectionMapper : IDataMapper
    {
        protected class CollectionInfoAndValue : IInfoAndValue
        {
            internal List<int> indexes;
            internal CollectionInfoAndValue(params int[] list)
            {
                indexes = new List<int>();
                for (int i = 0; i < list.Length; i++)
                {
                    indexes.Add(list[i]);   
                }
            }

            public object GetValuesForPlaceholders()
            {
                return indexes;
            }
        }
        protected abstract class CollectionOfSensors : ISensors
        {
            abstract internal IEnumerable sensorsCollection { get; } //null value possible
            internal bool needsUpdate;

            public List<Sensor> GetSensorsList() //null value not admitted
            {
                List<Sensor> toReturn = new List<Sensor>();
                foreach (ISensors isensors in sensorsCollection)
                {
                    if (isensors != null)
                    {
                        toReturn.AddRange(isensors.GetSensorsList());
                    }
                }
                return toReturn;
            }

        }
        protected abstract class CollectionOfActuators : IActuators
        {
            abstract internal IEnumerable actuatorsCollection { get; } //null value possible
            internal bool needsUpdates;

            public List<MonoBehaviourActuator> GetActuatorsList() //null value not admitted
            {
                List<MonoBehaviourActuator> toReturn = new List<MonoBehaviourActuator>();
                foreach (IActuators iactuators in actuatorsCollection)
                {
                    if (iactuators != null)
                    {
                        toReturn.AddRange(iactuators.GetActuatorsList());
                    }
                }
                return toReturn;
            }

        }
        public abstract bool Supports(Type t);
        public abstract Type ElementType(Type type);

        public List<int> GetAggregationStreamOperationsIndexes(Type type)
        {
            return MapperManager.GetAggregationStreamOperationsIndexes(ElementType(type));
        }
        public Type GetAggregationTypes(Type type = null)
        {
            return MapperManager.GetAggregationTypes(ElementType(type));
        }

        public bool NeedsAggregates(Type type)
        {
            return MapperManager.NeedsAggregates(ElementType(type));
        }

        public bool IsFinal(Type type)
        {
            return MapperManager.IsFinal(ElementType(type));
        }

        public bool IsTypeExpandable(Type type)
        {
            return MapperManager.IsTypeExpandable(ElementType(type));
        }
        public Dictionary<MyListString, KeyValuePair<Type, object>> RetrieveProperties(Type objectType, MyListString currentObjectPropertyHierarchy, object currentObject)
        {
            if (!Supports(objectType))
            {
                Debug.LogError(objectType + " is not supported.");
            }
            Type underlyingType = ElementType(objectType);
            MyListString rootName = new MyListString(currentObjectPropertyHierarchy.myStrings);
            if (!MapperManager.ExistsMapper(underlyingType))
            {
                rootName.Add(underlyingType.Name);
            }
            return MapperManager.RetrieveProperties(underlyingType, rootName, null);
        }

        protected void GenerateMapping(ref InstantiationInformation information, Type elementType)
        {
            /*
            if (information.mappingDone)
            {
                return;
            }
            string prepend = Placeholders(information);
            if (!MapperManager.ExistsMapper(elementType) || MapperManager.IsBasic(elementType))
            {
                prepend = ASPMapperHelper.AspFormat(information.residualPropertyHierarchy[0]) + "(" + information.temporaryMapping + prepend;
                string append = ")";
                information.prependMapping.Add(prepend);
                information.appendMapping.Insert(0, append);
                information.temporaryMapping = "";
            }
            else
            {
                information.temporaryMapping += prepend;
            }
            */
            if (information.mappingDone)
            {
                return;
            }
            string prepend = Placeholders(information);
            if (!MapperManager.ExistsMapper(elementType) || MapperManager.IsBasic(elementType))
            {
                prepend = information.temporaryMapping + prepend;
                information.prependMapping.Add(prepend);
                information.temporaryMapping = "";
            }
            else
            {
                information.temporaryMapping += prepend;
            }
        }


        protected static void UpdateResidualPropertyHierarchy(MyListString residualPropertyHierarchy, Type elementType)
        {
            if (MapperManager.ExistsMapper(elementType) && !MapperManager.IsBasic(elementType))
            {
                return;
            }
            if (residualPropertyHierarchy.Count > 0)
            {
                residualPropertyHierarchy.RemoveAt(0);
                if (residualPropertyHierarchy.Count > 0)//if it's not a primitive type array
                {
                    residualPropertyHierarchy.RemoveAt(0);
                }
            }

        }

        public string GetASPTemplate(ref InstantiationInformation information, List<string> variables)
        {
            AddVariables(information, variables);
            information.currentType = ElementType(information.currentType);
            GenerateMapping(ref information, information.currentType);
            IncreasePlaceholders(information);
            if (information.currentObjectOfTheHierarchy != null)
            {
                UpdateCurrentObjectOfTheHierarchy(information);
            }
            UpdateResidualPropertyHierarchy(information.residualPropertyHierarchy, information.currentType);
            return MapperManager.GetASPTemplate(ref information, variables);

        }

        protected ISensors InstantiateSensorsForElement(InstantiationInformation information, object actualCollection, params int[] indexes)
        {
            InstantiationInformation localInformation = AddLocalInformation(information, actualCollection, indexes);
            ISensors toReturn = MapperManager.InstantiateSensors(localInformation);
            if (!information.mappingDone && localInformation.mappingDone)
            {
                information.mappingDone = true;
                information.prependMapping = localInformation.prependMapping;
                information.appendMapping = localInformation.appendMapping;
            }
            return toReturn;
        }
        protected IActuators InstantiateActuatorsForElement(InstantiationInformation information, object actualCollection, params int[] indexes)
        {
            InstantiationInformation localInformation = AddLocalInformation(information, actualCollection, indexes);
            IActuators toReturn = MapperManager.InstantiateActuators(localInformation);
            if (!information.mappingDone && localInformation.mappingDone)
            {
                information.mappingDone = true;
                information.prependMapping = localInformation.prependMapping;
                information.appendMapping = localInformation.appendMapping;
            }
            return toReturn;
        }

        protected InstantiationInformation AddLocalInformation(InstantiationInformation information, object actualCollection, params int[] indexes)
        {
            InstantiationInformation localInformation = new InstantiationInformation(information);
            localInformation.hierarchyInfo.Add(new CollectionInfoAndValue(indexes));
            localInformation.currentObjectOfTheHierarchy = ElementOfTheCollection(actualCollection, indexes);
            return localInformation;
        }

        protected abstract object ElementOfTheCollection(object actualCollection, params int[] indexes);
        public abstract IActuators ManageActuators(InstantiationInformation information, IActuators instantiatedActuators);

        public abstract void UpdateSensor(Sensor sensor, object currentObject, MyListString residualPropertyHierarchy, int hierarchyLevel);

        public abstract ISensors ManageSensors(InstantiationInformation information, ISensors instantiatedSensors);

        public abstract ISensors InstantiateSensors(InstantiationInformation information);
        protected abstract void UpdateCurrentObjectOfTheHierarchy(InstantiationInformation information);

        protected abstract void IncreasePlaceholders(InstantiationInformation information);

        protected abstract void AddVariables(InstantiationInformation information, List<string> variables);
        public abstract IActuators InstantiateActuators(InstantiationInformation information);
        public abstract void SetPropertyValue(MonoBehaviourActuator actuator, MyListString residualPropertyHierarchy, object currentObject, object valueToSet, int hierarchyLevel);

        public bool NeedsSpecifications(Type type)
        {
            return true;
        }
        protected abstract string Placeholders(InstantiationInformation information);


    }
}
