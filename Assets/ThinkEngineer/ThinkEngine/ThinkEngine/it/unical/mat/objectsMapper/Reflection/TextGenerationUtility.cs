using it.unical.mat.embasp.languages;
using System;
using System.Collections.Generic;
using ThinkEngine.Mappers.ASPMappers;
using ThinkEngine.Mappers.BaseMappers;
using ThinkEngine.Mappers;

namespace ThinkEngine.ScriptGeneration
{
    internal class TextGenerationUtility
    {
        private TextGenerationUtility() { }

        #region SENSORS

        internal static string GenerateSensorScript(PropertyReflectionData reflectionData)
        {
            // GETTING_TEMPLATE
            string text = GetSensorTemplate();

            // USING NAME_SPACES
            text = ReplaceUsings(text, reflectionData);

            // CLASS_NAME
            text = ReplaceClassName(text, reflectionData);

            // FIELDS
            text = ReplaceFields(text, reflectionData);

            // INSTANTIATION
            text = ReplaceInitialization(text, reflectionData);

            // MANAGING
            text = ReplaceUpdate(text, reflectionData);

            // MANAGING
            text = ReplaceMap(text, reflectionData);

            // POST_PROCESSING
            text = PostProcessing(text);

            return text;
        }

        private static string GetSensorTemplate()
        {
            string template =

            "USINGS" +
            "namespace ThinkEngine" +
            "{" +
                "public class CLASSNAME : Sensor" +
                "{" +
                    "private int counter;" +
                    "private object specificValue;" +
                    "private Operation operation;" +
            "FIELDS" +
                    "public override void Initialize(SensorConfiguration sensorConfiguration)" +
                    "{" +
                        "this.gameObject = sensorConfiguration.gameObject;" +
                        "ready = true;" +
            "INITIALIZATION" +
                    "}" +
                    "public override void Destroy()" +
                    "{" +
                    "}" +
                    "public override void Update()" +
                    "{" +
            "UPDATE" +
                    "}" +
                    "public override string Map()" +
                    "{" +
            "MAP" +
                    "}" +
                "}" +
            "}";

            return template;
        }
        
        private static string ReplaceUsings(string text, PropertyReflectionData reflectionData)
        {
            string replace = string.Empty;

            foreach (string _namespace in reflectionData.Namespaces)
            {
                if (_namespace == null || _namespace.Equals(string.Empty)) continue;

                replace = string.Concat(replace, "" + string.Format("using {0};", _namespace));
            }

            return text.Replace("USINGS", replace);
        }

        private static string ReplaceClassName(string text, PropertyReflectionData reflectionData)
        {
            string replace = reflectionData.PropertyFeatures.PropertyAlias;
            return text.Replace("CLASSNAME", replace);
        }

        private static string ReplaceFields(string text, PropertyReflectionData reflectionData)
        {
            string replace = string.Empty;

            replace = string.Concat(replace, "private BasicTypeMapper mapper;");

            replace = string.Concat(replace, string.Format("private List<{0}> values = new List<{0}>();", GetRecursiveString("List<{0}>", reflectionData.NumberOfCollectionMappers, reflectionData.FinalTypeName)));

            return text.Replace("FIELDS", replace);
        }

        private static string ReplaceInitialization(string text, PropertyReflectionData reflectionData)
        {
            string replace = string.Empty;

            string mapping = ASPMapperHelper.AspFormat(reflectionData.PropertyFeatures.PropertyAlias) + "(" + ASPMapperHelper.AspFormat(reflectionData.SensorConfiguration.gameObject.name) + ",objectIndex(\"+index+\"),";
            int mappingIndex = 1;
            Type leafType = null;
            foreach (Type type in reflectionData.IDataMapperTypes.FindAll(x => x != null))
            {
                if (type.Equals(typeof(ASPListMapper)) || type.Equals(typeof(ASPArrayMapper)))
                {
                    mapping += "{" + (mappingIndex++) + "},";
                }
                else
                {
                    mapping += "{" + (mappingIndex++) + "},{" + (mappingIndex++) + "},";
                }
            }
            mapping += "{0}).";
            leafType = reflectionData.FinalType;
            //if (reflectionData.FinalMapperType.IsSubclassOf(typeof(BasicTypeMapper)))
            //{
            //    leafType = reflectionData.FinalType;

            //}
            //else if (reflectionData.FinalMapperType.IsSubclassOf(typeof(CollectionMapper)))
            //{
            //    leafType = ((CollectionMapper)reflectionData.FinalMapper).ElementType(reflectionData.FinalMapperType);

            //}

            replace = string.Concat(replace, string.Format("" +
                "int index = gameObject.GetInstanceID();" +
                "mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof({1}));" +
                "operation = mapper.OperationList()[{0}];" +
                "counter = {2};" +
                "mappingTemplate = \"{3}\" + Environment.NewLine;" +
                "", reflectionData.PropertyFeatures.operation, TypeNameOrAlias(leafType), reflectionData.PropertyFeatures.counter, mapping));

            if (reflectionData.PropertyFeatures.specificValue != null)
            {
                if (!reflectionData.PropertyFeatures.specificValue.Equals(""))
                {
                    replace = string.Concat(replace, string.Format("" +
                        "specificValue = Convert.ChangeType(\"{0}\", mapper.ConvertingType);", reflectionData.PropertyFeatures.specificValue));
                }
            }

            return text.Replace("INITIALIZATION", replace);
        }

        private static string ReplaceUpdate(string text, PropertyReflectionData reflectionData)
        {
            string replace = string.Empty;

            replace = string.Concat(replace, string.Format("" +
                "if(!ready)" +
                "{{" +
                    "return;" +
                "}}" +
                "if(!invariant || first)" +
                "{{" +
                    "first = false;" +
                    "{0}" +
                "}}", GetOperationToTargetProperty(1, reflectionData)));


            return text.Replace("UPDATE", replace);
        }

        private static string GetOperationToTargetProperty(int pos, PropertyReflectionData data)
        {
            string text = string.Empty;

            if (pos == data.Names.Count) return UpdateSensorValues(data);

            // Take next property

            string takeProperty = data.IDataMapperTypes[pos - 1] == null ? data.VariableNames[pos - 1] : data.VariableNames[pos - 1] + string.Format("[i_{0}]", pos - 1);
            takeProperty += (data.TypeNames[pos - 1].Equals("GameObject")) && data.AreComponent[pos] ? string.Format(".GetComponent<{0}>()", data.TypeNames[pos]) : data.Names[pos] != null ? string.Format(".{0}", data.Names[pos]) : "";

            text = string.Concat(text, string.Format("" +
                "{0} {1} = {2};" +
            "", data.TypeNames[pos], data.VariableNames[pos], takeProperty));

            // Check if property can be null

            if (!data.AreValueType[pos])
            {
                text = string.Concat(text, string.Format("" +
                    "if({0} == null)" +
                    "{{" +
                        "values{1}.Clear();" +
                        "{2};" +
                    "}}" +
                "", data.VariableNames[pos], GetCollectionElement(pos, data), GetNumberOfCollectionMapperBeforePosition(pos, data) == 0 ? "return" : "continue"));

                // Check if property need managing

                if (data.IDataMapperTypes[pos] != null)
                {
                    text = string.Concat(text, string.Format("" +
                        "else if({0} > values{1}.Count)" +
                        "{{" +
                            "for(int i = values{1}.Count; i < {0}; i++)" +
                            "{{" +
                                "values{1}.Add(new {2}());" +
                            "}}" +
                        "}}" +
                        "else if({0} < values{1}.Count)" +
                        "{{" +
                            "for(int i = {0}; i < values{1}.Count; i++)" +
                            "{{" +
                                "values{1}.RemoveAt(values{1}.Count - 1);" +
                            "}}" +
                        "}}" +
                    "", GetCollectionSize(data.IDataMapperTypes[pos], string.Format("{0}", data.VariableNames[pos])), GetCollectionElement(pos, data), GetRecursiveString("List<{0}>", data.NumberOfCollectionMappers - GetNumberOfCollectionMapperBeforePosition(pos, data), TypeNameOrAlias(data.FinalType))));

                    text = string.Concat(text, string.Format("" +
                        "for(int i_{0} = 0; i_{0} < {1}; i_{0}++)" +
                        "{{" +
                            "{2}" +
                        "}}" +
                    "", pos, GetCollectionSize(data.IDataMapperTypes[pos], data.VariableNames[pos]), GetOperationToTargetProperty(pos + 1, data)));
                }
                else
                {
                    text = string.Concat(text, GetOperationToTargetProperty(pos + 1, data));
                }
            }
            else
            {
                text = string.Concat(text, GetOperationToTargetProperty(pos + 1, data));
            }


            return text;
        }

        private static string UpdateSensorValues(PropertyReflectionData data)
        {
            string text = string.Empty;
            int pos = data.Names.Count - 1;
            int maxValue = data.SensorConfiguration.PropertyFeaturesList.Find(x => x.property.Equals(data.PropertyHierarchy)).windowWidth;

            string takeProperty = data.IDataMapperTypes[pos] == null ? data.VariableNames[pos] : data.VariableNames[pos] + string.Format("[i_{0}]", GetLastCollectionMapperPosition(pos, data));

            text = string.Concat(text, string.Format("" +
                "if (values{2}.Count == {1})" +
                "{{" +
                    "values{2}.RemoveAt(0);" +
                "}}" +
                "values{2}.Add({0});" +
            "", takeProperty, maxValue, GetCollectionElement(pos + 1, data)));

            return text;
        }

        private static string ReplaceMap(string text, PropertyReflectionData reflectionData)
        {
            string replace = string.Empty;

            if (reflectionData.NumberOfCollectionMappers == 0)
            {
                replace = string.Concat(replace, "" +
                    "object operationResult = operation(values, specificValue, counter);" +
                    "if(operationResult != null)" +
                    "{" +
                        "return string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult));" +
                    "}" +
                    "else" +
                    "{" +
                        "return \"\";" +
                    "}");
            }
            else if (reflectionData.NumberOfCollectionMappers > 0)
            {
                int forCounter = 0;
                string prefix = "string mapping = string.Empty;";
                string suffix = "";

                foreach (Type type in reflectionData.IDataMapperTypes.FindAll(x => x != null))
                {
                    prefix = string.Concat(prefix, string.Format("" +
                        "for(int i{0} = 0; i{0} < values{1}.Count; i{0}++)" +
                        "{{" +
                    "", forCounter, GetBrackets(forCounter)));
                    suffix = string.Concat("}", suffix);
                    forCounter++;
                }

                prefix = string.Concat(prefix, string.Format("" +
                    "object operationResult = operation(values{0}, specificValue, counter);" +
                    "if(operationResult != null)" +
                    "{{" +
                        "mapping = string.Concat(mapping, string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult),{1}));" +
                    "}}" +
                    "else" +
                    "{{" +
                        "mapping = string.Concat(mapping, string.Format(\"{{0}}\", Environment.NewLine));" +
                    "}}" +
                "", GetBrackets(forCounter), GetIndices(forCounter)));
                replace = string.Concat(prefix, suffix, "return mapping;");
            }

            return text.Replace("MAP", replace);
        }

        #endregion

        #region GENERAL

        // Adding newlines and tabs to an existing text
        private static string PostProcessing(string text)
        {
            // First pass (new lines)
            string newLine = Environment.NewLine;
            int numberOfOpenedBrackets = 0;
            bool inQuates = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (c == '{' && !inQuates)
                {
                    text = text.Insert(i, newLine);
                    i += newLine.Length;

                    text = text.Insert(i + 1, newLine);
                    i += newLine.Length;
                }
                else if (c == '}' && !inQuates && i < text.Length - 1)
                {
                    text = text.Insert(i + 1, newLine);
                    i += newLine.Length;
                }
                else if (c == ';')
                {
                    if (numberOfOpenedBrackets != 0) continue;

                    text = text.Insert(i + 1, newLine);
                    i += newLine.Length;
                }
                else if (c == '(' && !inQuates) numberOfOpenedBrackets++;
                else if (c == ')' && !inQuates) numberOfOpenedBrackets--;
                else if (c == '"') inQuates = !inQuates;
            }

            //Second pass(tab)
            string tab = "\t";
            int numberOfTabs = 0;
            inQuates = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (c == newLine[newLine.Length - 1])
                {
                    if (i < text.Length - 1 && text[i + 1] == '}')
                    {
                        numberOfTabs--;
                    }
                    string tabs = GetRecursiveString(tab + "{0}", numberOfTabs, "");
                    text = text.Insert(i + 1, tabs);
                    i += tabs.Length;
                }
                else if (c == '{' && !inQuates) numberOfTabs++;
                else if (c == '"') inQuates = !inQuates;
            }

            return text;
        }

        // Apply a format to itself <numberOfTimes> times and finally, uses <finalTexts> as args
        private static string GetRecursiveString(string format, int numberOfTimes, string finalText)
        {
            if (numberOfTimes <= 0) return finalText;
            return string.Format(format, GetRecursiveString(format, numberOfTimes - 1, finalText));
        }

        private static string GetBrackets(int v)
        {
            string toReturn = "";
            for (int i = 0; i < v; i++)
            {
                toReturn += "[i" + i + "]";
            }
            return toReturn;
        }

        private static object GetIndices(int forCounter)
        {
            string toReturn = "";
            for (int i = 0; i < forCounter - 1; i++)
            {
                toReturn += "i" + i + ",";
            }
            return toReturn + "i" + (forCounter - 1);
        }

        private static string GetCollectionElement(int pos, PropertyReflectionData data)
        {
            string text = string.Empty;

            int number = GetNumberOfCollectionMapperBeforePosition(pos, data);
            int count = 0;
            for (int i = 0; i < pos; i++)
            {
                if (data.IDataMapperTypes[i] != null)
                {
                    text = string.Concat(text, string.Format("[i_{0}]", i));
                    count++;

                    if (count >= number) break;
                }
            }

            return text;
        }

        private static int GetNumberOfCollectionMapperBeforePosition(int position, PropertyReflectionData data)
        {
            int count = 0;

            for (int i = 0; i < position; i++)
            {
                if (data.IDataMapperTypes[i] != null)
                {
                    count++;
                }
            }

            return count;
        }

        private static int GetLastCollectionMapperPosition(int position, PropertyReflectionData data)
        {
            int result = -1;

            for (int i = position; i >= 0; i--) 
            {
                if (data.IDataMapperTypes[i] == null) continue;
                result = i;
                break;
            }

            return result;
        }

        private static string GetCollectionSize(Type mapper, string propertyName)
        {
            string text = string.Empty;

            if (mapper.Equals(typeof(ASPListMapper)))
            {
                text = string.Format("{0}.Count", propertyName);
            }
            else if (mapper.Equals(typeof(ASPArrayMapper)))
            {
                text = string.Format("{0}.Length", propertyName);
            }
            else if (mapper.Equals(typeof(ASPArray2Mapper)))
            {
                text = string.Format("{0}.GetLength(0) * {0}.GetLength(1)", propertyName);
            }

            return text;
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
        private static string TypeNameOrAlias(Type type)
        {
            // Handle nullable value types
            Type nullbase = Nullable.GetUnderlyingType(type);
            if (nullbase != null)
                return TypeNameOrAlias(nullbase) + "?";

            // Handle arrays
            if (type.BaseType == typeof(System.Array))
            {
                string aliasName = TypeNameOrAlias(type.GetElementType());
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
                return RecursiveGenericTypeName(type);
            }

            // Default to CLR type name
            if (type.IsNested)
            {
                return NameForNestedType(type);
            }
            return type.Name;
        }

        private static string NameForNestedType(Type type)
        {
            string toReturn = type.DeclaringType + ".";
            if (type.DeclaringType.IsNested)
            {
                return toReturn + "." + NameForNestedType(type.DeclaringType) + "." + type.Name;
            }
            return toReturn + type.Name;
        }

        private static string RecursiveGenericTypeName(Type type)
        {
            if (!type.IsGenericType) return TypeNameOrAlias(type);

            Type[] genericArguments = type.GetGenericArguments();
            string genericTypeName = type.GetGenericTypeDefinition().Name.Remove(type.GetGenericTypeDefinition().Name.IndexOf("`"));

            string aliasName = genericTypeName;
            aliasName = string.Concat(aliasName, "<");
            for (int i = 0; i < genericArguments.Length; i++)
            {
                aliasName = string.Concat(aliasName, RecursiveGenericTypeName(genericArguments[i]));
                if (i < genericArguments.Length - 1)
                {
                    string.Concat(aliasName, ", ");
                }
            }
            aliasName = string.Concat(aliasName, ">");
            return aliasName;
        }

        #endregion
    }
}