using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using ThinkEngine.Mappers.BaseMappers;
using it.unical.mat.embasp.languages;
using ThinkEngine.Mappers;

namespace ThinkEngine.ScriptGeneration
{
    internal class ReflectionUtility
    {
        internal static bool PopulateReflectionData(PropertyReflectionData reflectionData)
        {
            reflectionData.Names.Add("gameObject");
            reflectionData.VariableNames.Add("gameObject");
            reflectionData.Types.Add(reflectionData.ObjectValue.GetType());
            reflectionData.TypeNames.Add(TypeNameOrAlias(reflectionData.ObjectValue.GetType(), reflectionData));
            reflectionData.AreComponent.Add(reflectionData.ObjectValue.GetType().IsSubclassOf(typeof(Component)));
            reflectionData.AreValueType.Add(reflectionData.ObjectValue.GetType().IsValueType);
            reflectionData.IDataMapperTypes.Add(null);

            reflectionData.Namespaces.Add(reflectionData.ObjectValue.GetType().Namespace);
            reflectionData.Namespaces.Add("System");
            reflectionData.Namespaces.Add("System.Collections.Generic");
            reflectionData.Namespaces.Add("ThinkEngine.Mappers");
            reflectionData.Namespaces.Add("static ThinkEngine.Mappers.OperationContainer");


            //Reflection
            if (!ReachPropertyByReflection(reflectionData.PropertyHierarchy.GetClone(), reflectionData.ObjectValue, reflectionData, out reflectionData.FinalType, out reflectionData.FinalMapper))
            {
                return false;
            }
            if (reflectionData.FinalMapper == null)
            {
                //Object 's value is null. Is this a problem?
                Debug.LogError(string.Format("Couldn't find a proper mapper for the target property of type {0}", reflectionData.FinalType.Name));
                return false;
            }


            reflectionData.PropertyFeatures = reflectionData.SensorConfiguration.PropertyFeaturesList.Find(x => x.property.Equals(reflectionData.PropertyHierarchy));
            reflectionData.FinalMapperType = reflectionData.FinalMapper.GetType();

            // Last property can be a recursive collection
            if (reflectionData.FinalMapperType.IsSubclassOf(typeof(CollectionMapper)))
            {
                GetResidualData(((CollectionMapper)reflectionData.FinalMapper).ElementType(reflectionData.Types[reflectionData.Types.Count - 1]), reflectionData);
                reflectionData.FinalType = reflectionData.Types[reflectionData.Types.Count - 1];
            }

            reflectionData.FinalTypeName = TypeNameOrAlias(reflectionData.FinalType, reflectionData);


            return true;
        }

        private static bool ReachPropertyByReflection(MyListString property, object currentObjectValue, PropertyReflectionData data, out Type finalType, out IDataMapper mapper)
        {
            for (int i = 0; i < property.Count; i++)
            {
                string propertyName = property[i];
            }

            Type currentType = null;
            finalType = null;
            mapper = null;
            currentType = currentObjectValue.GetType();
            string currentProperty = string.Empty;
            while (property.Count > 0)
            {
                currentProperty = property[0];
                currentObjectValue = RetrieveProperty(currentObjectValue, currentProperty, currentType, out currentType);
                IDataMapper tempMapper = MapperManager.GetMapper(currentType);
                if (tempMapper != null)
                {
                    mapper = tempMapper;
                }

                bool returnFalse = currentObjectValue == null && !ReachPropertyByReflectionByType(property, currentType, data, out finalType, out mapper);
                Type tempType = currentType;
                if (currentObjectValue == null)
                {
                    if (returnFalse)
                    {
                        //Object 's value is null. Is this a problem?
                        Debug.LogError(string.Format("Couldn't find {0}'s objectValue (null) during reflection!", currentProperty));
                        //TODO What if it is null?
                        return false;
                    }
                    return true;
                }
                else
                {
                    if (tempMapper != null && mapper is CollectionMapper collectionMapper)
                    {
                        data.IDataMapperTypes.Add(mapper.GetType());
                        data.NumberOfCollectionMappers++;
                        currentType = collectionMapper.ElementType(currentType);
                        data.Namespaces.Add(currentType.Namespace);
                    }
                    else
                    {
                        data.IDataMapperTypes.Add(null);
                    }

                    data.Names.Add(currentProperty);
                    data.VariableNames.Add(currentProperty + "_" + data.VariableNames.Count);
                    data.Types.Add(tempType);
                    data.TypeNames.Add(TypeNameOrAlias(tempType, data));
                    data.Namespaces.Add(tempType.Namespace);
                    data.AreComponent.Add(tempType.IsSubclassOf(typeof(Component)));
                    data.AreValueType.Add(tempType.IsValueType);

                    property.RemoveAt(0);
                    if (property.Count > 0 && currentType.Name == property[0])
                    {
                        property.RemoveAt(0);
                    }
                    if (property.Count == 0)
                    {
                        finalType = currentType;
                        if (mapper is CollectionMapper collectionMapper2)
                        {
                            mapper = MapperManager.GetMapper(collectionMapper2.ElementType(tempType));
                        }
                    }
                }
            }
            return true;
        }

        private static object RetrieveProperty(object currentObjectValue, string currentProperty, Type oldType, out Type currentType)
        {
            if (currentObjectValue == null)
            {
                currentType = oldType;
                return null;
            }
            MemberInfo[] members = currentObjectValue.GetType().GetMember(currentProperty, Utility.BindingAttr);
            //Debug.Log(string.Format("Members of {0} are : {1}", currentObjectValue.GetType(), string.Join(", ", (object[])members)));
            //If exists a currentObject's member with name currentProperty
            if (members.Length > 0)
            {
                FieldOrProperty fieldOrProperty = new FieldOrProperty(members[0]);
                //Debug.Log("First Member " + ((FieldInfo)members[0]).GetValue(currentObjectValue) + " New current Object value " + currentObjectValue);
                currentType = fieldOrProperty.Type();
                currentObjectValue = fieldOrProperty.GetValue(currentObjectValue);
                return currentObjectValue;
            }
            //If not let's search in the currentObject's components
            if (currentObjectValue is GameObject @object)
            {
                foreach (Component component in @object.GetComponents<Component>())
                {
                    if (component != null)
                    {
                        if (component.GetType().Name.Equals(currentProperty))
                        {
                            currentObjectValue = component;
                            currentType = component.GetType();
                            return currentObjectValue;
                        }
                    }
                }
            }
            currentType = oldType;
            return null;
        }

        //This method return true if it succeeds, false otherwise
        private static bool ReachPropertyByReflectionByType(MyListString property, Type currentType, PropertyReflectionData data, out Type finalType, out IDataMapper mapper)
        {
            finalType = null;
            mapper = null;
            string currentProperty = string.Empty;
            while (property.Count > 0)
            {
                currentProperty = property[0];
                currentType = RetrievePropertyByType(currentProperty, currentType);

                if (currentType == null)
                {
                    //Object 's value is null. Is this a problem?
                    Debug.LogError(string.Format("Couldn't find {0}'s in the {1} specification during reflection!", currentProperty, currentType));
                    //TODO What if it is null?
                    return false;
                }
                else
                {
                    Type tempType = currentType;
                    IDataMapper tempMapper = MapperManager.GetMapper(currentType);
                    if (tempMapper != null)
                    {
                        mapper = tempMapper;
                    }

                    if (tempMapper != null && mapper is CollectionMapper collectionMapper)
                    {
                        data.IDataMapperTypes.Add(mapper.GetType());
                        data.NumberOfCollectionMappers++;
                        currentType = collectionMapper.ElementType(currentType);
                        data.Namespaces.Add(currentType.Namespace);
                    }
                    else
                    {
                        data.IDataMapperTypes.Add(null);
                    }
                    data.Names.Add(currentProperty);
                    data.VariableNames.Add(currentProperty + "_" + data.VariableNames.Count);
                    data.Types.Add(tempType);
                    data.TypeNames.Add(TypeNameOrAlias(tempType, data));
                    data.Namespaces.Add(tempType.Namespace);
                    data.AreComponent.Add(tempType.IsSubclassOf(typeof(Component)));
                    data.AreValueType.Add(tempType.IsValueType);

                    property.RemoveAt(0);
                    if (property.Count > 0 && currentType.Name == property[0])
                    {
                        property.RemoveAt(0);
                    }
                    if (property.Count == 0)
                    {
                        finalType = currentType;
                        if (mapper is CollectionMapper collectionMapper2)
                        {
                            mapper = MapperManager.GetMapper(collectionMapper2.ElementType(tempType));
                        }
                    }
                }
            }
            return true;
        }

        private static Type RetrievePropertyByType(string currentProperty, Type currentType)
        {
            MemberInfo[] members = currentType.GetMember(currentProperty, Utility.BindingAttr);
            if (members.Length > 0)
            {
                FieldOrProperty fieldOrProperty = new FieldOrProperty(members[0]);
                return fieldOrProperty.Type();
            }
            else
            {
                return null;
            }
        }

        // This is the set of types from the C# keyword list.
        private static Dictionary<Type, string> _typeAlias = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(object), "object" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(string), "string" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            // Yes, this is an odd one.  Technically it's a type though.
            { typeof(void), "void" }
        };

        // Extract from a Type object a string <type> which can be use in a c# code
        // https://stackoverflow.com/questions/56352299/gettype-return-int-instead-of-system-int32
        private static string TypeNameOrAlias(Type type, PropertyReflectionData data)
        {
            // Handle nullable value types
            Type nullbase = Nullable.GetUnderlyingType(type);
            if (nullbase != null)
                return TypeNameOrAlias(nullbase, data) + "?";

            // Handle arrays
            if (type.BaseType == typeof(System.Array))
            {
                string aliasName = TypeNameOrAlias(type.GetElementType(), data);
                aliasName = string.Concat(aliasName, "[");
                for (int i = 0; i < type.GetArrayRank() - 1; i++)
                {
                    aliasName = string.Concat(aliasName, ",");
                }
                aliasName = string.Concat(aliasName, "]");
                return aliasName;
            }
            // Lookup alias for type
            if (_typeAlias.TryGetValue(type, out string alias))
                return alias;

            //GMDG adding generic handling
            if (type.IsGenericType)
            {
                return RecursiveGenericTypeName(type, data);
            }

            // Default to CLR type name
            if (type.IsNested)
            {
                return NameForNestedType(type, data);
            }
            return type.Name;
        }

        private static string NameForNestedType(Type type, PropertyReflectionData data)
        {
            string toReturn = type.DeclaringType + ".";
            data.Namespaces.Add(type.DeclaringType.Namespace);
            if (type.DeclaringType.IsNested)
            {
                return toReturn + "." + NameForNestedType(type.DeclaringType, data) + "." + type.Name;
            }
            return toReturn + type.Name;
        }

        private static string RecursiveGenericTypeName(Type type, PropertyReflectionData data)
        {
            if (!type.IsGenericType) return TypeNameOrAlias(type, data);

            Type[] genericArguments = type.GetGenericArguments();
            string genericTypeName = type.GetGenericTypeDefinition().Name.Remove(type.GetGenericTypeDefinition().Name.IndexOf("`"));

            string aliasName = genericTypeName;
            aliasName = string.Concat(aliasName, "<");
            for (int i = 0; i < genericArguments.Length; i++)
            {
                aliasName = string.Concat(aliasName, RecursiveGenericTypeName(genericArguments[i], data));
                if (i < genericArguments.Length - 1)
                {
                    string.Concat(aliasName, ", ");
                }
            }
            aliasName = string.Concat(aliasName, ">");
            return aliasName;
        }

        // Get Residual Data if last property is a collection
        private static void GetResidualData(Type type, PropertyReflectionData data) 
        {
            IDataMapper mapper = MapperManager.GetMapper(type);

            if (mapper == null) return;

            data.Names.Add(null);
            data.VariableNames.Add("v_" + data.VariableNames.Count);
            data.Types.Add(type);
            data.TypeNames.Add(TypeNameOrAlias(type, data));
            data.Namespaces.Add(type.Namespace);
            data.AreComponent.Add(type.IsSubclassOf(typeof(Component)));
            data.AreValueType.Add(type.IsValueType);

            if (mapper is BasicTypeMapper basicTypeMapper) 
            {
                data.IDataMapperTypes.Add(null);
            }
            else if (mapper is CollectionMapper collectionMapper)
            {
                type = collectionMapper.ElementType(type);

                data.IDataMapperTypes.Add(mapper.GetType());
                data.NumberOfCollectionMappers++;

                GetResidualData(type, data);
            }
        }
    }

    internal class PropertyReflectionData
    {
        internal SensorConfiguration SensorConfiguration;
        internal PropertyFeatures PropertyFeatures;
        internal object ObjectValue;
        internal MyListString PropertyHierarchy;

        internal List<string> Names = new List<string>();
        internal List<string> VariableNames = new List<string>();
        internal List<Type> Types = new List<Type>();
        internal List<string> TypeNames = new List<string>();
        internal HashSet<string> Namespaces = new HashSet<string>();
        internal List<bool> AreComponent = new List<bool>();
        internal List<bool> AreValueType = new List<bool>();
        internal List<Type> IDataMapperTypes = new List<Type>();
        internal int NumberOfCollectionMappers = 0;

        internal Type FinalMapperType;
        internal IDataMapper FinalMapper;
        internal Type FinalType;
        internal string FinalTypeName;

        public PropertyReflectionData(MyListString propertyHierarchy, object objectValue, SensorConfiguration sensorConfiguration)
        {
            PropertyHierarchy = propertyHierarchy;
            ObjectValue = objectValue;
            SensorConfiguration = sensorConfiguration;
        }

        public override string ToString()
        {
            string text = string.Empty;

            text = string.Format("{0}{1}{2}", text, "SensorConfiguration: \t\t" + SensorConfiguration._configurationName, Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "PropertyFeatures: \t\t" + PropertyFeatures.PropertyAlias, Environment.NewLine);
            text = string.Format("{0}{1}{2}", text,  ObjectValue != null ? "ObjectValue: \t\t\t" + ObjectValue.GetType() : "ObjectValue: \t" + string.Empty, Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "PropertyHierarchy: \t\t" + string.Join(", ", PropertyHierarchy.myStrings), Environment.NewLine);

            text = string.Format("{0}{1}{2}", text, "Names: \t\t\t\t" + string.Join(", ", Names), Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "VariablesNames: \t\t" + string.Join(", ", VariableNames), Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "Types: \t\t\t\t" + string.Join(", ", Types), Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "TypeNames: \t\t\t" + string.Join(", ", TypeNames), Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "Namespaces: \t\t\t" + string.Join(", ", Namespaces), Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "AreComponents: \t\t" + string.Join(", ", AreComponent), Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "AreValueType: \t\t\t" + string.Join(", ", AreValueType), Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "IDataMapperTypes: \t\t" + string.Join(", ", IDataMapperTypes), Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "Number Of Collection Mappers: \t" + NumberOfCollectionMappers, Environment.NewLine);

            text = string.Format("{0}{1}{2}", text, "FinalMapperType: \t\t" + FinalMapperType, Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, FinalMapper != null ? "FinalMapper: \t\t\t" + FinalMapper.GetType() : "FinalMapper: \t" + string.Empty, Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "FinalType: \t\t\t" + FinalType, Environment.NewLine);
            text = string.Format("{0}{1}{2}", text, "FinalTypeName: \t\t\t" + FinalTypeName, Environment.NewLine);

            return text;
        }
    }
}

