using ThinkEngine.Mappers;
using ThinkEngine.Mappers.BaseMappers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ThinkEngine
{
    internal class MapperManager
    {

        static readonly ConcurrentDictionary<Type, IDataMapper> metMappers; //each type that is supported for sensors is associated with the actual data mapper 
                                                                            //that is a derived class of DataMapper. The derived class register itself to this dictionary
        static readonly List<Type> unsupportedTypes;
        static readonly List<IDataMapper> mappers;
        static MapperManager()
        {
            metMappers = new ConcurrentDictionary<Type, IDataMapper>();
            mappers = new List<IDataMapper>();
            unsupportedTypes = new List<Type>();
            RegisterMappers();
        }

        #region PUBLIC STATIC METHODS

        #region COMMON METHODS
        internal static bool IsFinal(Type type)
        {
            if (ExistsMapper(type))
            {
                return metMappers[type].IsFinal(type);
            }
            return false;
        }
        internal static Dictionary<MyListString, KeyValuePair<Type, object>> RetrieveProperties(Type objectType, MyListString currentObjectPropertyHierarchy, object currentObject)
        {
            if (ExistsMapper(objectType))
            {
                return metMappers[objectType].RetrieveProperties(objectType, currentObjectPropertyHierarchy, currentObject);
            }
            else
            {
                return RetrieveGeneralProperties(objectType, currentObjectPropertyHierarchy, currentObject);
            }
        }
        private static IDataMapper RetrieveAdditionalInformation(ref InstantiationInformation information, bool generateMapping = false)
        {
            object currentObject = information.currentObjectOfTheHierarchy;
            //Debug.Log(information.currentObjectOfTheHierarchy+"2");
            IDataMapper mapper = RetrieveMapper(ref information.residualPropertyHierarchy, ref currentObject, out information.currentType);
            if (mapper != null)
            {
                information.currentObjectOfTheHierarchy = currentObject;
                if (generateMapping)
                {
                    GenerateMapping(ref information);
                }
            }
            return mapper;
        }

        
        internal static IDataMapper GetMapper(Type type)
        {
            return ExistsMapper(type) ? metMappers[type] : null;
        }

        private static IDataMapper RetrieveAdditionalInformationByType(ref InstantiationInformation information, bool generateMapping)
        {
            IDataMapper mapper = RetrieveMapperByType(ref information.residualPropertyHierarchy, ref information.currentType);
            if (mapper != null)
            {
                if (generateMapping)
                {
                    GenerateMapping(ref information);
                }
            }

            return mapper;
        }
        private static void GenerateMapping(ref InstantiationInformation information)
        {
            /*int upTo = information.propertyHierarchy.Count - information.residualPropertyHierarchy.Count;
            string prepend = "";
            string append = "";
            for (int i = information.prependMapping.Count; i < upTo; i++)
            {
                prepend += ASPMapperHelper.AspFormat(information.propertyHierarchy[i]) + "(";
                append = ")" + append;
            }
            if (!prepend.Equals(""))
            {
                information.prependMapping.Add(prepend);
                information.appendMapping.Add(append);
            }*/

        }
        internal static IDataMapper RetrieveMapper(ref MyListString residualPropertyHierarchy, ref object currentObject, out Type currentType)
        {
            currentType = null;
            if (currentObject != null)
            {
                //Debug.Log(currentObject);
                if (ExistsMapper(currentObject.GetType()))
                {
                    //Debug.Log("esiste il mapper per l'oggetto "+currentObject);
                    currentType = currentObject.GetType();
                    return metMappers[currentObject.GetType()];
                }
                /*else
                {
                    Debug.Log("non esiste il mapper per l'oggetto " + currentObject);
                }*/
            }
            //Debug.Log(residualPropertyHierarchy.ToString());
            while (residualPropertyHierarchy.Count > 0)
            {
                string currentProperty = residualPropertyHierarchy[0];
                //Debug.Log(currentProperty +" proprietà");
                currentObject = RetrieveProperty(currentObject, currentProperty, out currentType);
                //Debug.Log(currentObject+ " C");
                if (currentType != null && ExistsMapper(currentType))
                {
                    return metMappers[currentType];
                }
                else
                {
                    if (currentObject == null)
                    {
                        return null;
                    }
                }
                residualPropertyHierarchy.RemoveAt(0);
            }
            return null;
        }


        internal static IDataMapper RetrieveMapperByType(ref MyListString residualPropertyHierarchy, ref Type currentType)
        {
            if (currentType != null)
            {
                if (ExistsMapper(currentType))
                {
                    return metMappers[currentType];
                }
            }
            while (residualPropertyHierarchy.Count > 0)
            {
                string currentProperty = residualPropertyHierarchy[0];
                //Debug.Log(currentProperty);
                //Debug.Log(currentType);
                RetrievePropertyByType(currentProperty, ref currentType);
                if (currentType != null && ExistsMapper(currentType))
                {
                    return metMappers[currentType];
                }
                else
                {
                    if (currentType == null)
                    {
                        return null;
                    }
                }
                residualPropertyHierarchy.RemoveAt(0);
            }
            return null;
        }

        private static void RetrievePropertyByType(string currentProperty, ref Type currentType)
        {
            //Debug.Log("sono dentro RetrievePropertyByType");
            MemberInfo[] members = currentType.GetMember(currentProperty, Utility.BindingAttr);
            if (members.Length > 0)
            {
                FieldOrProperty fieldOrProperty = new FieldOrProperty(members[0]);
                currentType = fieldOrProperty.Type();
            }
            else
            {
                currentType = null;
            }
        }

        internal static object RetrieveProperty(object currentObject, string currentProperty, out Type currentType)
        {
            if (currentObject == null)
            {
                currentType = null;
                return null;
            }
            //Debug.Log(currentProperty);
            MemberInfo[] members = currentObject.GetType().GetMember(currentProperty, Utility.BindingAttr);
            //Debug.Log(members.Length);
            if (members.Length > 0)
            {
                //Debug.Log(currentObject + " BAU");
                FieldOrProperty fieldOrProperty = new FieldOrProperty(members[0]);
                currentObject = fieldOrProperty.GetValue(currentObject);
                //Debug.Log(currentObject + " BAU1");
                currentType = fieldOrProperty.Type();
                return currentObject;
            }
            if (currentObject is GameObject @object)
            {
                //Debug.Log(currentObject + " BAU2");
                foreach (Component component in @object.GetComponents<Component>())
                {
                    if (component != null)
                    {
                        if (component.GetType().Name.Equals(currentProperty))
                        {
                            //Debug.Log(currentObject+" 5");
                            currentObject = component;
                            currentType = component.GetType();
                            return currentObject;
                        }
                    }
                }
            }
            currentType = null;
            return null;
        }
        internal static bool NeedsAggregates(Type type)
        {
            if (ExistsMapper(type))
            {
                return metMappers[type].NeedsAggregates(type);
            }
            return false;
        }
        internal static bool NeedsSpecifications(Type type)
        {
            if (ExistsMapper(type))
            {
                return metMappers[type].NeedsSpecifications(type);
            }
            return true;
        }
        internal static Type GetAggregationTypes(Type type)
        {
            if (ExistsMapper(type))
            {
                if (metMappers[type].NeedsAggregates(type))
                {
                    return metMappers[type].GetAggregationTypes(type);
                }
            }
            return null;
        }
        internal static List<int> GetAggregationStreamOperationsIndexes(Type type)
        {
            if (ExistsMapper(type))
            {
                if (metMappers[type].NeedsAggregates(type))
                {
                    return metMappers[type].GetAggregationStreamOperationsIndexes(type);
                }
            }
            return new List<int>();
        }

        private static Dictionary<MyListString, KeyValuePair<Type, object>> RetrieveGeneralProperties(Type objectType, MyListString currentObjectPropertyHierarchy, object currentObject)
        {
            if (currentObject != null && !currentObject.GetType().Equals(objectType))
            {
                throw new Exception("Actual object type differs from declared one.");
            }
            Dictionary<MyListString, KeyValuePair<Type, object>> toReturn = new Dictionary<MyListString, KeyValuePair<Type, object>>();
            foreach (MemberInfo member in objectType.GetMembers(Utility.BindingAttr))
            {
                bool isField = member.MemberType.Equals(MemberTypes.Field);
                bool isProperty = member.MemberType.Equals(MemberTypes.Property);
                if (member != null && (isField || isProperty))
                {
                    MyListString toAdd = new MyListString(currentObjectPropertyHierarchy.myStrings);
                    toAdd.Add(member.Name);
                    FieldOrProperty fieldOrProperty = new FieldOrProperty(member);
                    object propertyValue;
                    try
                    {
                        propertyValue = currentObject != null ? fieldOrProperty.GetValue(currentObject) : null;
                    }
                    catch
                    {
                        propertyValue = null;
                    }
                    if (toReturn.ContainsKey(toAdd))
                    {
                        continue;
                    }
                    KeyValuePair<Type, object> propertyPair = new KeyValuePair<Type, object>(fieldOrProperty.Type(), propertyValue);
                    toReturn[toAdd] = propertyPair;
                }
            }
            if (currentObject is GameObject @object)
            {
                foreach (Component component in @object.GetComponents(typeof(Component)))
                {
                    if (component != null)
                    {
                        MyListString toAdd = new MyListString(currentObjectPropertyHierarchy.myStrings);
                        toAdd.Add(component.GetType().Name);
                        if (toReturn.ContainsKey(toAdd))
                        {
                            continue;
                        }
                        KeyValuePair<Type, object> propertyPair = new KeyValuePair<Type, object>(component.GetType(), component);
                        toReturn.Add(toAdd, propertyPair);
                    }
                }
            }
            return toReturn;
        }

        internal static string GetASPTemplate(string propertyAlias, GameObject gameObject, MyListString propertyHierarchy, bool isSensor = false)
        {
            InstantiationInformation information = new InstantiationInformation
            {
                propertyHierarchy = propertyHierarchy,
                residualPropertyHierarchy = new MyListString(propertyHierarchy.myStrings),
                currentObjectOfTheHierarchy = gameObject
            };
            information.firstPlaceholder = 0;
            IDataMapper mapper = RetrieveAdditionalInformation(ref information, true);

            if (mapper == null)
            {
                RetrieveProperty(gameObject, propertyHierarchy[0], out information.currentType);
                information.currentObjectOfTheHierarchy = null;
                information.residualPropertyHierarchy = new MyListString(propertyHierarchy.GetRange(1, propertyHierarchy.Count - 1).myStrings);
                information.firstPlaceholder = 0;
                //information.prependMapping.Add(ASPMapperHelper.AspFormat(propertyHierarchy[0]) + "(");
                //information.appendMapping.Insert(0, ")");
                mapper = RetrieveAdditionalInformationByType(ref information, true);
            }
            if (mapper != null)
            {
                if (information.currentObjectOfTheHierarchy != null)
                {
                    information.currentType = information.currentObjectOfTheHierarchy.GetType();
                }
                string partial = mapper.GetASPTemplate(ref information, new List<string>());
                CompleteMapping(propertyAlias, gameObject, isSensor, ref partial);
                return partial;
            }
            information.prependMapping.Clear();
            information.appendMapping.Clear();
            return "";
        }

        internal static string GetASPTemplate(ref InstantiationInformation information, List<string> variables)
        {
            IDataMapper mapper = null;
            InstantiationInformation informationClone = new InstantiationInformation(information);
            if (information.currentObjectOfTheHierarchy != null)
            {
                mapper = RetrieveAdditionalInformation(ref information, true);
            }
            if (mapper == null)
            {
                mapper = RetrieveAdditionalInformationByType(ref informationClone, true);
                information = informationClone;
            }
            if (mapper != null)
            {
                return mapper.GetASPTemplate(ref information, variables);
            }
            information.prependMapping.Clear();
            information.appendMapping.Clear();
            return "";
        }

        private static void CompleteMapping(string propertyAlias, GameObject gameObject, bool isSensor, ref string partialMapping)
        {
            string cleanPropertyAlias = ASPMapperHelper.AspFormat(propertyAlias);
            partialMapping = cleanPropertyAlias + "(" + ASPMapperHelper.AspFormat(gameObject.name) + ",objectIndex(Index)," + partialMapping;
            partialMapping += ")";
            if (!isSensor)
            {
                partialMapping = "setOnActuator(" + partialMapping;
                partialMapping += ") :-objectIndex(" + cleanPropertyAlias + ", Index), .";
            }
            else
            {
                partialMapping = "%" + partialMapping;
                partialMapping += ".";
            }
            partialMapping += Environment.NewLine;
        }

        internal static void RegisterMappers()
        {
            foreach (Type type in typeof(IDataMapper).Assembly.GetTypes())
            {
                CheckAndRegister(type, mappers);
            }
        }
        private static void CheckAndRegister(Type derivedType, IList mappers)
        {
            if (typeof(IDataMapper).IsAssignableFrom(derivedType))
            {
                PropertyInfo instanceProperty = derivedType.GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                if (instanceProperty != null)
                {
                    object instance = instanceProperty.GetValue(null);
                    if (instance != null)
                    {
                        mappers.Add(instance);
                    }
                }
            }
        }
        internal static bool IsTypeExpandable(Type type)
        {
            if (ExistsMapper(type))
            {
                return metMappers[type].IsTypeExpandable(type);
            }
            return true;
        }
        internal static bool ExistsMapper(Type type)
        {
            return !unsupportedTypes.Contains(type) && (metMappers.ContainsKey(type) || IsSupportedByMapper(type));
        }
        private static bool IsSupportedByMapper(Type type)
        {
            foreach (IDataMapper mapper in mappers)
            {
                if (mapper.Supports(type))
                {
                    metMappers[type] = mapper;
                    return true;
                }
            }
            unsupportedTypes.Add(type);
            return false;
        }
        #endregion

        #region SENSORS METHODS
        internal static ISensors InstantiateSensors(InstantiationInformation information)
        {
            //Debug.Log(information.Mapping());
            ISensors toReturn = null;
            IDataMapper mapper = RetrieveAdditionalInformation(ref information, !information.mappingDone);
            //Debug.Log(information.propertyHierarchy.ToString());
            if (mapper != null)
            {
                ISensors sensors = mapper.InstantiateSensors(information);
                if (sensors != null)
                {
                    toReturn = sensors;
                }
            }
            if (!information.mappingDone)
            {
                information.prependMapping.Clear();
                information.appendMapping.Clear();
            }
            return toReturn;
        }
        internal static ISensors ManageSensors(InstantiationInformation information, ISensors sensors)
        {
            IDataMapper mapper = RetrieveAdditionalInformation(ref information, !information.mappingDone);
            ISensors toReturn = null;
            if (mapper != null)
            {
                toReturn = mapper.ManageSensors(information, sensors);
            }
            if (!information.mappingDone)
            {
                information.prependMapping.Clear();
                information.appendMapping.Clear();
            }
            return toReturn;
        }

        internal static void UpdateSensor(Sensor sensor, object currentObject, MyListString residualPropertyHierarchy, int level)
        {
            IDataMapper mapper = RetrieveMapper(ref residualPropertyHierarchy, ref currentObject, out _);
            if (mapper != null)
            {
                mapper.UpdateSensor(sensor, currentObject, residualPropertyHierarchy, level);
            }
        }
        #endregion

        #region ACTUATORS METHODS
        internal static IActuators InstantiateActuators(InstantiationInformation information)
        {
            IActuators toReturn = null;
            IDataMapper mapper = RetrieveAdditionalInformation(ref information, !information.mappingDone);
            if (mapper != null)
            {
                toReturn = mapper.InstantiateActuators(information);
            }
            if (!information.mappingDone)
            {
                information.prependMapping.Clear();
                information.appendMapping.Clear();
            }
            return toReturn;
        }
        internal static IActuators ManageActuators(InstantiationInformation information, IActuators actuators)
        {
            IActuators toReturn = null;
            IDataMapper mapper = RetrieveAdditionalInformation(ref information, !information.mappingDone);
            if (mapper != null)
            {
                toReturn = mapper.ManageActuators(information, actuators);
            }
            if (!information.mappingDone)
            {
                information.prependMapping.Clear();
                information.appendMapping.Clear();
            }
            return toReturn;
        }

        internal static bool IsBasic(Type type)
        {
            if (ExistsMapper(type))
            {
                return metMappers[type] is BasicTypeMapper;
            }
            return false;
        }

        internal static void SetPropertyValue(MonoBehaviourActuator actuator, MyListString residualPropertyHierarchy, object currentObject, object valueToSet, int level)
        {
            MyListString residualPropertyButLast = new MyListString(residualPropertyHierarchy.myStrings.GetRange(0, residualPropertyHierarchy.Count - 1));
            string lastProperty = residualPropertyHierarchy[residualPropertyHierarchy.Count - 1];
            object originalObject = currentObject;
            IDataMapper mapper = RetrieveMapper(ref residualPropertyHierarchy, ref currentObject, out _);
            if (mapper != null)
            {
                if (!IsBasic(currentObject.GetType()))
                {
                    mapper.SetPropertyValue(actuator, residualPropertyHierarchy, currentObject, valueToSet, level);
                }
                else
                {
                    mapper = RetrieveMapper(ref residualPropertyButLast, ref originalObject, out _);
                    MemberInfo[] members = originalObject.GetType().GetMember(lastProperty, Utility.BindingAttr);
                    if (members.Length > 0)
                    {
                        FieldOrProperty fieldOrProperty = new FieldOrProperty(members[0]);
                        Type propertyType = fieldOrProperty.Type();
                        fieldOrProperty.SetValue(originalObject, Convert.ChangeType(valueToSet, propertyType));
                    }
                }
            }

        }

        internal static object GetConvertedValue(object valueToSet)
        {
            if (ExistsMapper(valueToSet.GetType()))
            {
                return ((BasicTypeMapper)metMappers[valueToSet.GetType()]).GetConvertedValue(valueToSet);
            }
            return null;
        }
        #endregion

        #endregion

    }
}