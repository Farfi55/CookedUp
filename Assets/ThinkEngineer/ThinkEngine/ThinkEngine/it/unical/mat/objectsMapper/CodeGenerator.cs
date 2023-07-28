#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using System.IO;
using ThinkEngine.Mappers;
using ThinkEngine.Mappers.ASPMappers;
using ThinkEngine.Mappers.BaseMappers;
using static ThinkEngine.Mappers.OperationContainer;

namespace ThinkEngine
{
    internal class CodeGenerator
    {
        private static int gid = 0;

        //Paths
        private static readonly string rootPath = string.Format("{0}", Path.Combine(Application.dataPath, "Scripts"));
        private static readonly string generatedCodeRelativePath = Path.Combine("Assets", "Scripts", "GeneratedCode");
        private static readonly string generatedCodePath = string.Format(Path.Combine("{0}", "GeneratedCode"), rootPath);
        private static readonly string templateRelativePath = Path.Combine("Assets", "Scripts", "SensorTemplate.txt");

        //Helping data structures
        private static List<string> propertyHierarchyNames;
        private static List<string> propertyHierarchyTypeNames;
        private static List<bool> arePropertiesComponent; //knows if the relative property is a component or a property/field
        private static List<bool> arePropertiesPrimitive;

        //Helping variables
        private static string sensorName;
        private static Type finalType;
        private static IDataMapper mapper;
        private static Type mapperType;
        private static SensorConfiguration currentSensorConfiguration;
        private static MyListString currentPropertyHierarchy;
        private static PropertyFeatures currentPropertyFeatures;

        private CodeGenerator() { }

        //Generate the MonobehaviourSensorManager
        internal static void GenerateCode(List<MyListString> toMapProperties, object objectValue, SensorConfiguration sensorConfiguration)
        {
            if (sensorConfiguration.ConfigurationName.Equals(string.Empty))
            {
                Debug.LogError("SensorConfiguration name can't be empty!");
                return;
            }

            currentSensorConfiguration = sensorConfiguration;

            foreach (MyListString propertyHierarchy in toMapProperties)
            {
                currentPropertyHierarchy = propertyHierarchy;
                currentPropertyFeatures = currentSensorConfiguration.PropertyFeaturesList.Find(x => x.property.Equals(currentPropertyHierarchy));

                propertyHierarchyNames = new List<string>();
                propertyHierarchyTypeNames = new List<string>();
                arePropertiesComponent = new List<bool>(); //knows if the relative property is a component or a property/field
                arePropertiesPrimitive = new List<bool>();

                object currentObjectValue = objectValue;

                //Reflection
                if (!ReachPropertyByReflection(propertyHierarchy.GetClone(), currentObjectValue, out finalType, out mapper))
                {
                    continue;
                }
                if (mapper == null)
                {
                    //Object 's value is null. Is this a problem?
                    Debug.LogError(string.Format("Couldn't find a proper mapper for the target property of type {0}", finalType.Name));
                    continue;
                }

                mapperType = mapper.GetType();

                sensorName = GenerateSensorName(sensorConfiguration);

                Debug.Log(string.Join(",", propertyHierarchyNames));
                Debug.Log(string.Join(",", propertyHierarchyTypeNames));
                Debug.Log(string.Join(",", arePropertiesComponent));
                Debug.Log(string.Join(",", arePropertiesPrimitive));

                TextAsset templateTextFile = AssetDatabase.LoadAssetAtPath(templateRelativePath, typeof(TextAsset)) as TextAsset;
                if (templateTextFile == null)
                {
                    Debug.LogError(string.Format("Template file not found in {0}!", templateRelativePath));
                    return;
                }
                string content = CreateText(templateTextFile.text);
                string path = Path.Combine(generatedCodePath, sensorName + ".cs");
                
                // confirm overwrite
                if (File.Exists(path) && !EditorUtility.DisplayDialog(string.Format("Generated code file already exists in {0}", generatedCodeRelativePath), "Do you want to overwite it?", "Yes", "No"))
                {
                    return;
                }
                
                // create folder if not exists
                if (!Directory.Exists(generatedCodePath)) 
                    Directory.CreateDirectory(generatedCodePath);
                
                File.WriteAllText(path, content);

                // Refresh the unity asset database
                AssetDatabase.ImportAsset(generatedCodeRelativePath, ImportAssetOptions.ForceSynchronousImport);
                AssetDatabase.Refresh();
            }
        }

        #region TEXT_GENERATION
        private static string GenerateSensorName(SensorConfiguration sensorConfiguration)
        {
            string sensorName = string.Format(sensorConfiguration.ConfigurationName + "{0}", gid);
            gid++;

            sensorName = char.ToUpper(sensorName[0]) + sensorName.Substring(1);

            return sensorName;
        }

        private static string CreateText(string text)
        {
            //CLASS_NAME
            text = ReplaceClassName(text);

            //FIELDS
            text = ReplaceFields(text);

            //INSTANTIATION
            text = ReplaceInitialization(text);

            //MANAGING
            text = ReplaceUpdate(text);

            //MANAGING
            text = ReplaceMap(text);

            return text;
        }

        private static string ReplaceClassName(string text)
        {
            return text.Replace("CLASS_NAME", sensorName);
        }

        private static string ReplaceFields(string text)
        {
            string addedText = string.Empty;

            addedText = string.Concat(addedText, "" +
                string.Format("" +
                "{0}private BasicTypeMapper mapper;{2}", GetTabs(0), TypeNameOrAlias(finalType), Environment.NewLine));


            if (mapperType.IsSubclassOf(typeof(BasicTypeMapper)))
            {
                addedText = string.Concat(addedText, "" +
                    string.Format("" +
                    "{0}private List<{1}> values = new List<{1}>();{2}", GetTabs(0), TypeNameOrAlias(finalType), Environment.NewLine));
            }
            else if (mapperType.IsSubclassOf(typeof(CollectionMapper)))
            {
                addedText = string.Concat(addedText, "" +
                    string.Format("" +
                    "{0}private List<List<{1}>> values = new List<List<{1}>>();{2}" +
                    "{0}private List<bool> isIndexActive = new List<bool>();{2}", GetTabs(0), TypeNameOrAlias(((CollectionMapper)mapper).ElementType(finalType)), Environment.NewLine));

                //Indexing for the sensor
                if (mapperType.Equals(typeof(ASPArrayMapper)) || mapperType.Equals(typeof(ASPListMapper)))
                {
                    addedText = string.Concat(addedText, "" +
                        string.Format("{0}private List<int> indicies = new List<int>();\n", GetTabs(0)));

                }
                else if (mapperType.Equals(typeof(ASPArray2Mapper)))
                {
                    addedText = string.Concat(addedText, "" +
                        string.Format("{0}private List<(int, int)> indicies = new List<(int, int)>();\n", GetTabs(0)));
                }
            }

            return text.Replace("FIELDS", addedText);
        }

        private static string ReplaceInitialization(string text)
        {
            string addedText = string.Empty;

            string mapping = string.Empty;
            int index = currentSensorConfiguration.gameObject.GetComponent<IndexTracker>().CurrentIndex;

            if (mapperType.IsSubclassOf(typeof(BasicTypeMapper)))
            {
                addedText = string.Concat(addedText,
                    string.Format("" +
                    "{0}mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof({3}));{1}" +
                    "{0}operation = mapper.OperationList()[{2}];{1}" +
                    "{0}counter = {4};{1}", GetTabs(1), Environment.NewLine, currentPropertyFeatures.operation, TypeNameOrAlias(finalType), currentPropertyFeatures.counter));

                mapping = currentPropertyFeatures.PropertyAlias + "(" + ASPMapperHelper.AspFormat(currentSensorConfiguration.gameObject.name) + ",objectIndex(" + index + ")," + "{0}" + ").";
                addedText = string.Concat(addedText,
                    string.Format("" +
                    "{0}mappingTemplate = \"{1}\" + Environment.NewLine;{2}", GetTabs(1), mapping, Environment.NewLine));
            }
            else if (mapperType.IsSubclassOf(typeof(CollectionMapper)))
            {
                addedText = string.Concat(addedText,
                    string.Format("" +
                    "{0}mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof({3}));{1}" +
                    "{0}operation = mapper.OperationList()[{2}];{1}" +
                    "{0}counter = {4};{1}", GetTabs(1), Environment.NewLine, currentPropertyFeatures.operation, TypeNameOrAlias(((CollectionMapper)mapper).ElementType(finalType)), currentPropertyFeatures.counter));


                if (mapperType.Equals(typeof(ASPArrayMapper)))
                {
                    mapping = ASPMapperHelper.AspFormat(currentPropertyFeatures.PropertyAlias) + "(" + ASPMapperHelper.AspFormat(currentSensorConfiguration.gameObject.name) + ",objectIndex(" + index + ")," + "{0},{1}" + ").";
                    addedText = string.Concat(addedText,
                        string.Format("" +
                        "{0}mappingTemplate = \"{1}\" + Environment.NewLine;{2}", GetTabs(1), mapping, Environment.NewLine));

                    addedText = string.Concat(addedText, GetOperationToTargetProperty(1));

                    addedText = string.Concat(addedText, "" +
                        string.Format("" +
                        "{0}for(int i = 0; i < {1}{2}.Length; i++)\n" +
                        "{0}{{\n" +
                            "{3}indicies.Add((i));\n" +
                            "{3}isIndexActive.Add(true);\n" +
                            "{3}values.Add(new List<{4}>());\n" +
                        "{0}}}\n", GetTabs(1), propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1, GetTabs(2), TypeNameOrAlias(((CollectionMapper)mapper).ElementType(finalType))));
                }
                else if (mapperType.Equals(typeof(ASPArray2Mapper)))
                {
                    mapping = ASPMapperHelper.AspFormat(currentPropertyFeatures.PropertyAlias) + "(" + ASPMapperHelper.AspFormat(currentSensorConfiguration.gameObject.name) + ",objectIndex(" + index + ")," + "{0},{1},{2}" + ").";
                    addedText = string.Concat(addedText,
                        string.Format("" +
                        "{0}mappingTemplate = \"{1}\" + Environment.NewLine;{2}", GetTabs(1), mapping, Environment.NewLine));

                    addedText = string.Concat(addedText, GetOperationToTargetProperty(1));

                    addedText = string.Concat(addedText, "" +
                        string.Format("" +
                        "{0}for(int i = 0; i < {1}{2}.GetLength(0); i++)\n" +
                        "{0}{{\n" +
                            "{3}for(int j = 0; j < {1}{2}.GetLength(1); j++)\n" +
                            "{3}{{\n" +
                                "{4}indicies.Add((i, j));\n" +
                                "{4}isIndexActive.Add(true);\n" +
                                "{4}values.Add(new List<{5}>());\n" +
                            "{3}}}\n" +
                        "{0}}}\n", GetTabs(1), propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1, GetTabs(2), GetTabs(3), TypeNameOrAlias(((CollectionMapper)mapper).ElementType(finalType))));
                }
                else if (mapperType.Equals(typeof(ASPListMapper)))
                {
                    mapping = ASPMapperHelper.AspFormat(currentPropertyFeatures.PropertyAlias) + "(" + ASPMapperHelper.AspFormat(currentSensorConfiguration.gameObject.name) + ",objectIndex(" + index + ")," + "{0},{1}" + ").";
                    addedText = string.Concat(addedText,
                        string.Format("" +
                        "{0}mappingTemplate = \"{1}\" + Environment.NewLine;", GetTabs(1), mapping));

                    addedText = string.Concat(addedText, GetOperationToTargetProperty(1));

                    addedText = string.Concat(addedText, "" +
                        string.Format("" +
                        "{0}for(int i = 0; i < {1}{2}.Count; i++)\n" +
                        "{0}{{\n" +
                            "{3}indicies.Add((i));\n" +
                            "{3}isIndexActive.Add(true);\n" +
                            "{3}values.Add(new List<{4}>());\n" +
                        "{0}}}\n", GetTabs(1), propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1, GetTabs(2), TypeNameOrAlias(((CollectionMapper)mapper).ElementType(finalType))));
                }
            }

            if (currentPropertyFeatures.specificValue != null)
            {
                if (!currentPropertyFeatures.specificValue.Equals(""))
                {
                    addedText = string.Concat(addedText,
                        string.Format("" +
                        "{0}specificValue = Convert.ChangeType(\"{2}\", mapper.ConvertingType);{1}", GetTabs(1), Environment.NewLine, currentPropertyFeatures.specificValue));
                }
            }

            return text.Replace("INITIALIZATION", addedText);
        }

        private static string ReplaceUpdate(string text)
        {
            string addedText = string.Empty;

            addedText = string.Concat(addedText, "" +
                string.Format("" +
                "{0}if(!ready){4}" +
                "{0}{{{4}" +
                    "{1}return;{4}" +
                "{0}}}{4}" +
                "{0}if(!invariant || first){4}" +
                "{0}{{{4}" +
                    "{1}first = false;{4}" +
                    "{2}" +
                    "{3}" +
                "{0}}}", GetTabs(1), GetTabs(2), CheckIfSensorNeedsManaging(), UpdateSensorValues(), Environment.NewLine));

            return text.Replace("UPDATE", addedText);
        }

        private static string ReplaceMap(string text)
        {
            string addedText = string.Empty;

            if (mapperType.IsSubclassOf(typeof(BasicTypeMapper)))
            {
                addedText = string.Concat(addedText, "" +
                    string.Format("" +
                    "{0}object operationResult = operation(values, specificValue, counter);{1}" +
                    "{0}return string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult));", GetTabs(1), Environment.NewLine));
            }
            else if (mapperType.IsSubclassOf(typeof(CollectionMapper)))
            {
                string indicies = string.Empty;

                if (mapperType.Equals(typeof(ASPArrayMapper)) || mapperType.Equals(typeof(ASPListMapper)))
                {
                    indicies = " indicies[i],";
                }
                else if (mapperType.Equals(typeof(ASPArray2Mapper)))
                {
                    indicies = " indicies[i].Item1, indicies[i].Item2,";
                }

                addedText = string.Concat(addedText, "" +
                    string.Format("" +
                    "{0}string mapping = string.Empty;{1}" +
                    "{0}for(int i = 0; i < values.Count; i++){1}" +
                    "{0}{{{1}" +
                        "{2}if(!isIndexActive[i]) continue;{1}" +
                        "{2}object operationResult = operation(values[i], specificValue, counter);{1}" +
                        "{2}mapping = string.Concat(mapping, string.Format(mappingTemplate,{3} BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult)));{1}" +
                    "{0}}}{1}" +
                    "{0}return mapping;", GetTabs(1), Environment.NewLine, GetTabs(2), indicies));
            }

            return text.Replace("MAP", addedText);
        }

        private static string CheckIfSensorNeedsManaging()
        {
            string text = string.Empty;

            text = string.Concat(text, GetOperationToTargetProperty(2));

            if (mapperType.IsSubclassOf(typeof(CollectionMapper)))
            {
                //Checking if something changed
                text = string.Concat(text, "" +
                    string.Format("" +
                        "{0}if({3} > isIndexActive.Count)\n" +
                        "{0}{{\n" +
                            "{1}for(int i = isIndexActive.Count; i < {3}; i++)\n" +
                            "{1}{{\n" +
                                "{2}indicies.Add({5});\n" +
                                "{2}isIndexActive.Add(true);\n" +
                                "{2}values.Add(new List<{4}>());\n" +
                            "{1}}}\n" +
                        "{0}}}\n" +
                        "{0}else if({3} < isIndexActive.Count)\n" +
                        "{0}{{\n" +
                            "{1}for(int i = {3}; i < isIndexActive.Count; i++)\n" +
                            "{1}{{\n" +
                                "{2}indicies.RemoveAt(isIndexActive.Count - 1);\n" +
                                "{2}isIndexActive.RemoveAt(isIndexActive.Count - 1);\n" +
                                "{2}values.RemoveAt(isIndexActive.Count - 1);\n" +
                            "{1}}}\n" +
                        "{0}}}\n", GetTabs(2), GetTabs(3), GetTabs(4), GetCollectionSize(), TypeNameOrAlias(((CollectionMapper)mapper).ElementType(finalType)), GetCollectionIndex()));

                if (!((CollectionMapper)mapper).ElementType(finalType).IsPrimitive)
                {
                    //The single element can be null
                    text = string.Concat(text, "" +
                        string.Format("" +
                        "{0}for(int i = 0; i < values.Count; i++)\n" +
                        "{0}{{\n" +
                            "{1}if({3} == null && isIndexActive[i])\n" +
                            "{1}{{\n" +
                                "{2}isIndexActive[i] = false;\n" +
                            "{1}}}\n" +
                            "{1}else if({3} != null && !isIndexActive[i])\n" +
                            "{1}{{\n" +
                                "{2}isIndexActive[i] = true;\n" +
                            "{1}}}\n" +
                        "{0}}}", GetTabs(2), GetTabs(3), GetTabs(4), GetCollectionElement()));
                }
            }

            return text;
        }

        private static string GetOperationToTargetProperty(int baseOfTabs)
        {
            string text = string.Empty;

            int i = 0;

            if (arePropertiesComponent[i])
            {
                text = string.Concat(text, "" +
                    string.Format("{3}{0} {1}{2} = gameObject.GetComponent<{0}>();\n", propertyHierarchyTypeNames[0], propertyHierarchyNames[0], i, GetTabs(baseOfTabs)));
            }
            else
            {
                text = string.Concat(text, "" +
                    string.Format("{3}{0} {1}{2} = gameObject.{1};\n", propertyHierarchyTypeNames[0], propertyHierarchyNames[0], i, GetTabs(baseOfTabs)));
            }

            if (!arePropertiesPrimitive[i])
            {
                text = string.Concat(text, "" +
                    string.Format("{0}if({1}{2} == null) return;\n", GetTabs(baseOfTabs), propertyHierarchyNames[0], i));
            }

            for (i = 1; i < propertyHierarchyNames.Count; i++)
            {
                text = string.Concat(text, "" +
                    string.Format("{3}{0} {1}{2} = {4}{5}", propertyHierarchyTypeNames[i], propertyHierarchyNames[i], i, GetTabs(baseOfTabs), propertyHierarchyNames[i - 1], i - 1));

                if (propertyHierarchyTypeNames[i - 1].Equals("GameObject") && arePropertiesComponent[i])
                {
                    text = string.Concat(text, "" +
                        string.Format(".GetComponent<{0}>();\n", propertyHierarchyTypeNames[i]));
                }
                else
                {
                    text = string.Concat(text, "" +
                        string.Format(".{0};\n", propertyHierarchyNames[i]));
                }


                if (arePropertiesPrimitive[i]) continue;

                text = string.Concat(text, "" +
                    string.Format("{0}if({1}{2} == null) return;\n", GetTabs(baseOfTabs), propertyHierarchyNames[i], i));
            }

            text = string.Concat(text, "\n");

            return text;
        }

        private static string GetTabs(int count)
        {
            int baseCount = 2;
            string text = string.Empty;

            for (int i = 0; i < baseCount + count; i++)
            {
                text += "\t";
            }

            return text;
        }

        private static string GetCollectionElement()
        {
            string text = string.Empty;

            if (mapperType.Equals(typeof(ASPListMapper)))
            {
                text = string.Format("{0}{1}[i]", propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1);
            }
            else if (mapperType.Equals(typeof(ASPArrayMapper)))
            {
                text = string.Format("(Array){0}{1}.GetValue(i)", propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1);
            }
            else if (mapperType.Equals(typeof(ASPArray2Mapper)))
            {
                text = string.Format("(Array){0}{1}.GetValue(i % {0}{1}.GetLength(1), i / {0}{1}.GetLength(1))", propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1);
            }

            return text;
        }

        private static string GetCollectionIndex()
        {
            string text = string.Empty;

            if (mapperType.Equals(typeof(ASPListMapper)) || mapperType.Equals(typeof(ASPArrayMapper)))
            {
                text = "i";
            }
            else if (mapper.GetType().Equals(typeof(ASPArray2Mapper)))
            {
                text = string.Format("(i % {0}{1}.GetLength(1), i / {0}{1}.GetLength(1))", propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1);
            }

            return text;
        }

        private static string GetCollectionSize()
        {
            string text = string.Empty;

            if (mapperType.Equals(typeof(ASPListMapper)))
            {
                text = string.Format("{0}{1}.Count", propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1);
            }
            else if (mapperType.Equals(typeof(ASPArrayMapper)))
            {
                text = string.Format("{0}{1}.Length", propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1);
            }
            else if (mapperType.Equals(typeof(ASPArray2Mapper)))
            {
                text = string.Format("{0}{1}.GetLength(0) * {0}{1}.GetLength(1)", propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1);
            }

            return text;
        }

        private static string UpdateSensorValues()
        {
            string text = string.Empty;

            int maxValue = currentSensorConfiguration.PropertyFeaturesList.Find(x => x.property.Equals(currentPropertyHierarchy)).windowWidth;

            //Updating
            if (mapperType.IsSubclassOf(typeof(BasicTypeMapper)))
            {
                text = string.Concat(text, "" +
                    string.Format("" +
                        "{0}if (values.Count == {4})\n" +
                        "{0}{{\n" +
                            "{3}values.RemoveAt(0);\n" +
                        "{0}}}\n" +
                        "{0}values.Add({1}{2});\n", GetTabs(2), propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1, GetTabs(3), maxValue));

            }
            else if (mapperType.IsSubclassOf(typeof(CollectionMapper)))
            {
                text = string.Concat(text, "" +
                    string.Format("" +
                    "{0}for(int i = 0; i < values.Count; i++)\n" +
                    "{0}{{\n" +
                        "{1}if (values[i].Count == {4})\n" +
                        "{1}{{\n" +
                            "{2}values[i].RemoveAt(0);\n" +
                        "{1}}}\n" +
                        "{1}{3}" +
                    "{0}}}\n", GetTabs(2), GetTabs(3), GetTabs(4), UpdateSensorValue(), maxValue));
            }

            return text;
        }

        private static string UpdateSensorValue()
        {
            string text = string.Empty;

            if (mapperType.Equals(typeof(ASPListMapper)) || mapperType.Equals(typeof(ASPArrayMapper)))
            {
                text = string.Format("values[i].Add({0}{1}[indicies[i]]);\n", propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1);
            }
            else if (mapperType.Equals(typeof(ASPArray2Mapper)))
            {
                text = string.Format("values[i].Add({0}{1}[indicies[i].Item1, indicies[i].Item2]);\n", propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1);
            }

            return text;
        }

        #endregion

        #region REFLECTION

        //This method return true if it succeeds, false otherwise
        private static bool ReachPropertyByReflection(MyListString property, object currentObjectValue, out Type finalType, out IDataMapper mapper)
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
                if (mapper == null)
                {
                    IDataMapper tempMapper = MapperManager.GetMapper(currentType);
                    if (tempMapper != null)
                    {
                        mapper = tempMapper;
                    }
                }

                propertyHierarchyNames.Add(currentProperty);
                propertyHierarchyTypeNames.Add(TypeNameOrAlias(currentType));
                arePropertiesComponent.Add(currentType.IsSubclassOf(typeof(Component)));
                arePropertiesPrimitive.Add(currentType.IsPrimitive);

                property.RemoveAt(0);

                if (currentObjectValue == null)
                {
                    if (!ReachPropertyByReflectionByType(propertyHierarchyNames, propertyHierarchyTypeNames, arePropertiesComponent, arePropertiesPrimitive, property, currentType, out finalType, out mapper))
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

                    if (property.Count == 0)
                    {
                        finalType = currentType;
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

            //If exists a currentObject's member with name currentProperty
            if (members.Length > 0)
            {
                FieldOrProperty fieldOrProperty = new FieldOrProperty(members[0]);
                currentObjectValue = fieldOrProperty.GetValue(currentObjectValue);
                currentType = fieldOrProperty.Type();
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
        private static bool ReachPropertyByReflectionByType(List<string> propertyHierarchyNames, List<string> propertyHierarchyTypeNames, List<bool> arePropertiesComponent, List<bool> arePropertiesPrimitive, MyListString property, Type currentType, out Type finalType, out IDataMapper mapper)
        {
            finalType = null;
            mapper = null;
            string currentProperty = string.Empty;
            while (property.Count > 0)
            {
                currentProperty = property[0];
                currentType = RetrievePropertyByType(currentProperty, currentType);
                Debug.Log(currentType);
                if (currentType == null)
                {
                    //Object 's value is null. Is this a problem?
                    Debug.LogError(string.Format("Couldn't find {0}'s in the {1} specification during reflection!", currentProperty, currentType));
                    //TODO What if it is null?
                    return false;
                }
                else
                {
                    if (mapper == null)
                    {
                        IDataMapper tempMapper = MapperManager.GetMapper(currentType);
                        if (tempMapper != null)
                        {
                            mapper = tempMapper;
                            Debug.Log(mapper.GetType());
                        }
                    }

                    propertyHierarchyNames.Add(currentProperty);
                    propertyHierarchyTypeNames.Add(TypeNameOrAlias(currentType));
                    arePropertiesComponent.Add(currentType.IsSubclassOf(typeof(Component)));
                    arePropertiesPrimitive.Add(currentType.IsPrimitive);

                    if (property.Count == 1)
                    {
                        finalType = currentType;
                    }
                }
                property.RemoveAt(0);
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
            return type.Name;
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
#endif
