using System;
using System.Collections.Generic;

namespace ThinkEngine.Mappers.BaseMappers
{
    public interface IDataMapper
    {
        bool NeedsSpecifications(Type type);
        bool NeedsAggregates(Type type);
        bool IsTypeExpandable(Type type);
        bool Supports(Type type);
        bool IsFinal(Type t);
        Type GetAggregationTypes(Type type = null);
        List<int> GetAggregationStreamOperationsIndexes(Type type);
        Dictionary<MyListString, KeyValuePair<Type, object>> RetrieveProperties(Type objectType, MyListString currentObjectPropertyHierarchy, object currentObject);
        ISensors InstantiateSensors(InstantiationInformation information);
        void UpdateSensor(Sensor sensor, object currentObject, MyListString residualPropertyHierarchy, int level);
        ISensors ManageSensors(InstantiationInformation information, ISensors sensors);
        IActuators InstantiateActuators(InstantiationInformation information);
        IActuators ManageActuators(InstantiationInformation information, IActuators actuators);
        void SetPropertyValue(MonoBehaviourActuator actuator, MyListString propertyHierarchy, object currentObject, object valueToSet, int level);// Set the value of the property or field (propertyHierarchy) for the object actualObject
        string GetASPTemplate(ref InstantiationInformation information, List<string> variables);
    }
}