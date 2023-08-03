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
        private static List<string> propertyHierarchyTypeNamespaces;
        private static List<bool> arePropertiesComponent; //knows if the relative property is a component or a property/field
        private static List<bool> arePropertiesPrimitive;
        private static List<Type> iDataMapperTypes; // null if the property is not a collection

        //Helping variables
        private static int numberOfCollectionMappers;
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
                propertyHierarchyTypeNamespaces = new List<string>();
                arePropertiesComponent = new List<bool>(); //knows if the relative property is a component or a property/field
                arePropertiesPrimitive = new List<bool>();
                iDataMapperTypes = new List<Type>();
                numberOfCollectionMappers = 0;

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

                sensorName = GenerateSensorName(currentPropertyFeatures.PropertyAlias);

                //Debug.Log(string.Join(", ", propertyHierarchyNames));
                //Debug.Log(string.Join(", ", propertyHierarchyTypeNames));
                //Debug.Log(string.Join(", ", propertyHierarchyTypeNamespaces));
                //Debug.Log(string.Join(", ", arePropertiesComponent));
                //Debug.Log(string.Join(", ", arePropertiesPrimitive));
                //Debug.Log(string.Join(", ", iDataMapperTypes));
                //Debug.Log(string.Format("Number of CollectionMappers : {0}", numberOfCollectionMappers));
                //Debug.Log(mapperType);

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
                //Debug.Log("overwriting file "+path);
                // create folder if not exists
                if (!Directory.Exists(generatedCodePath))
                    Directory.CreateDirectory(generatedCodePath);

                File.WriteAllText(path, content);

                
            }
            // Refresh the unity asset database
            AssetDatabase.ImportAsset(generatedCodeRelativePath, ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.Refresh();
        }

        #region TEXT_GENERATION
        private static string GenerateSensorName(string propertyName)
        {
            /*string sensorName = string.Format(sensorConfiguration.ConfigurationName + "{0}", gid);
            gid++;

            sensorName = char.ToUpper(sensorName[0]) + sensorName.Substring(1);

            return sensorName;*/
            return propertyName;
        }

        private static string CreateText(string text)
        {
            //USING NAME_SPACES
            text = ReplaceUsing(text);

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

        private static string ReplaceUsing(string text)
        {

            string addedText = string.Empty;

            propertyHierarchyTypeNamespaces.Add("System");
            propertyHierarchyTypeNamespaces.Add("System.Collections.Generic");
            propertyHierarchyTypeNamespaces.Add("ThinkEngine.Mappers");
            propertyHierarchyTypeNamespaces.Add("static ThinkEngine.Mappers.OperationContainer");

            propertyHierarchyTypeNamespaces = propertyHierarchyTypeNamespaces.Distinct().ToList(); //Deleting duplicates from namespaces

            for (int i = 0; i < propertyHierarchyTypeNamespaces.Count; i++)
            {
                if (propertyHierarchyTypeNamespaces[i] == null || propertyHierarchyTypeNamespaces[i].Equals(string.Empty)) continue;
                addedText = string.Concat(addedText, "" +
                    string.Format("using {0};{1}", propertyHierarchyTypeNamespaces[i], Environment.NewLine));
            }

            return text.Replace("USINGS", addedText);
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

            addedText = string.Concat(addedText, "" +
                string.Format("" +
                "{0}private List<{2}> values = new List<{2}>();{1}", GetTabs(0), Environment.NewLine, GetRecursiveString("List<{0}>", numberOfCollectionMappers)));


            //if (mapperType.IsSubclassOf(typeof(BasicTypeMapper)))
            //{
            //    addedText = string.Concat(addedText, "" +
            //        string.Format("" +
            //        "{0}private List<{1}> values = new List<{1}>();{2}", GetTabs(0), TypeNameOrAlias(finalType), Environment.NewLine));
            //}
            //else if (mapperType.IsSubclassOf(typeof(CollectionMapper)))
            //{
            //    addedText = string.Concat(addedText, "" +
            //        string.Format("" +
            //        "{0}private List<List<{1}>> values = new List<List<{1}>>();{2}" +
            //        "{0}private List<bool> isIndexActive = new List<bool>();{2}", GetTabs(0), TypeNameOrAlias(((CollectionMapper)mapper).ElementType(finalType)), Environment.NewLine));

            //    //Indexing for the sensor
            //    if (mapperType.Equals(typeof(ASPArrayMapper)) || mapperType.Equals(typeof(ASPListMapper)))
            //    {
            //        addedText = string.Concat(addedText, "" +
            //            string.Format("{0}private List<int> indicies = new List<int>();\n", GetTabs(0)));

            //    }
            //    else if (mapperType.Equals(typeof(ASPArray2Mapper)))
            //    {
            //        addedText = string.Concat(addedText, "" +
            //            string.Format("{0}private List<(int, int)> indicies = new List<(int, int)>();\n", GetTabs(0)));
            //    }
            //}

            return text.Replace("FIELDS", addedText);
        }

        private static string GetRecursiveString(string text, int numberOfTimes)
        {
            if (numberOfTimes == 0) return TypeNameOrAlias(finalType);
            return string.Format(text, GetRecursiveString(text, numberOfTimes - 1));
        }

        private static string ReplaceInitialization(string text)
        {
            string addedText = string.Empty;

            string mapping = ASPMapperHelper.AspFormat(currentPropertyFeatures.PropertyAlias) + "(" + ASPMapperHelper.AspFormat(currentSensorConfiguration.gameObject.name) + ",objectIndex(\"+index+\"),";
            int mappingIndex = 1;
            Type leafType = null;
            foreach(Type t in iDataMapperTypes.FindAll(x=>x!=null))
            {
                if (t.Equals(typeof(ASPListMapper)) || t.Equals(typeof(ASPArrayMapper)))
                {
                    mapping += "{" + (mappingIndex++) + "},";
                }
                else
                {
                    mapping += "{" + (mappingIndex++) + "},{" + (mappingIndex++) + "},";
                }
            }
            mapping += "{0}).";
            if (mapperType.IsSubclassOf(typeof(BasicTypeMapper)))
            {
                leafType = finalType;

            }
            else if (mapperType.IsSubclassOf(typeof(CollectionMapper)))
            {
                leafType = ((CollectionMapper)mapper).ElementType(finalType);

            }

            addedText = string.Concat(addedText,
                string.Format("" +
                "{0}int index = gameObject.GetInstanceID();{1}" +
                "{0}mapper = (BasicTypeMapper)MapperManager.GetMapper(typeof({3}));{1}" +
                "{0}operation = mapper.OperationList()[{2}];{1}" +
                "{0}counter = {4};{1}", GetTabs(1), Environment.NewLine, currentPropertyFeatures.operation, TypeNameOrAlias(leafType), currentPropertyFeatures.counter));

            addedText = string.Concat(addedText,
                string.Format("" +
                "{0}mappingTemplate = \"{1}\" + Environment.NewLine;{2}", GetTabs(1), mapping, Environment.NewLine));
           /* { 
                addedText = string.Concat(addedText, GetOperationToTargetProperty(1));
                if (mapperType.Equals(typeof(ASPArrayMapper)) || mapperType.Equals(typeof(ASPListMapper)))
                {
                    string collectionSizeField;
                    if (mapperType.Equals(typeof(ASPArrayMapper)))
                    {
                        collectionSizeField = "Length";
                    }
                    else
                    {
                        collectionSizeField = "Count";

                    }
                    addedText = string.Concat(addedText, "" +
                        string.Format("" +
                        "{0}for(int i = 0; i < {1}{2}."+collectionSizeField+"; i++)\n" +
                        "{0}{{\n" +
                            "{3}indicies.Add((i));\n" +
                            "{3}isIndexActive.Add(true);\n" +
                            "{3}values.Add(new List<{4}>());\n" +
                        "{0}}}\n", GetTabs(1), propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1, GetTabs(2), TypeNameOrAlias(leafType)));
                }
                else if (mapperType.Equals(typeof(ASPArray2Mapper)))
                {
                    mapping += "{1},{2}";


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
                        "{0}}}\n", GetTabs(1), propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1, GetTabs(2), GetTabs(3), TypeNameOrAlias(leafType)));
                }
            }
           */
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
                "{0}if(!ready){3}" +
                "{0}{{{3}" +
                    "{1}return;{3}" +
                "{0}}}{3}" +
                "{0}if(!invariant || first){3}" +
                "{0}{{{3}" +
                    "{1}first = false;{3}" +
                    "{2}" +
                "{0}}}", GetTabs(1), GetTabs(2), CheckIfSensorNeedsManaging(), Environment.NewLine));

            return text.Replace("UPDATE", addedText);
        }

        private static string ReplaceMap(string text)
        {
            string addedText = string.Empty;
            if (iDataMapperTypes.FindAll(x=>x!=null).Count==0)
            {
                addedText = string.Concat(addedText, "" +
                    string.Format("" +
                    "{0}object operationResult = operation(values, specificValue, counter);{2}" +
                    "{0}if(operationResult != null){2}" +
                    "{0}{{{2}" +
                        "{1}return string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult));{2}" +
                    "{0}}}{2}" +
                    "{0}else{2}" +
                    "{0}{{{2}" +
                        "{1}return \"\";{2}" +
                    "{0}}}", GetTabs(1), GetTabs(2), Environment.NewLine));
            }
            else
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
                int forCounter = 0;
                string prefix = string.Format("" +
                        "{0}string mapping = string.Empty;{1}", GetTabs(forCounter + 1), Environment.NewLine);
                string suffix = "";
                for (int i=0; i < iDataMapperTypes.Count; i++)
                {
                    if (iDataMapperTypes[i] == null) continue;
                    prefix = string.Concat(prefix,
                        string.Format("" +
                        "{0}for( int i{1}=0; i{1}<values{2}.Count;i{1}++){3}"+
                        "{0}{{{3}", GetTabs(forCounter+1),forCounter, GetBrackets(forCounter), Environment.NewLine));
                    suffix = string.Concat(string.Format("{0}}}{1}", GetTabs(forCounter+1), Environment.NewLine),suffix);
                    forCounter++;

                }
                prefix = string.Concat(prefix,
                        string.Format("" +
                        "{0}object operationResult = operation(values{4}, specificValue, counter);{3}" +
                        "{0}if(operationResult != null){3}" +
                        "{0}{{{3}" +
                            "{2}mapping = string.Concat(mapping, string.Format(mappingTemplate, BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult),{5}));{3}" +
                        "{0}}}{3}" +
                        "{0}else{3}" +
                        "{0}{{{3}" +
                            "{2}mapping = string.Concat(mapping, string.Format(\"{{0}}\", Environment.NewLine));{3}" +
                        "{0}}}{3}",GetTabs(forCounter+1),GetTabs(forCounter),GetTabs(forCounter+2),Environment.NewLine,GetBrackets(forCounter),GetIndices(forCounter)));
                addedText = string.Concat(prefix, suffix,string.Format("{0}return mapping;{1}", GetTabs(1), Environment.NewLine));
                /*
                addedText = string.Concat(addedText, "" +
                    string.Format("" +
                    "{0}string mapping = string.Empty;{1}" +
                    "{0}for(int i = 0; i < values.Count; i++){1}" +
                    "{0}{{{1}" +
                        "{2}if(!isIndexActive[i]) continue;{1}" +
                        "{2}object operationResult = operation(values[i], specificValue, counter);{1}" +
                        "{2}if(operationResult != null){1}" +
                        "{2}{{{1}" +
                            "{4}mapping = string.Concat(mapping, string.Format(mappingTemplate,{3} BasicTypeMapper.GetMapper(operationResult.GetType()).BasicMap(operationResult)));{1}" +
                        "{2}}}{1}" +
                        "{2}else{1}" +
                        "{2}{{{1}" +
                            "{4}mapping = string.Concat(mapping, string.Format(\"{{0}}\", Environment.NewLine));{1}" +
                        "{2}}}{1}" +
                    "{0}}}{1}" +
                    "{0}return mapping;", GetTabs(1), Environment.NewLine, GetTabs(2), indicies, GetTabs(3)));
                */
            }

            return text.Replace("MAP", addedText);
        }

        private static object GetIndices(int forCounter)
        {
            string toReturn = "";
            for(int i=0; i<forCounter-1; i++)
            {
                toReturn += "i" + i + ",";
            }
            return toReturn + "i"+(forCounter-1);
        }

        private static string GetBrackets(int v)
        {
            string toReturn = "";
            for(int i=0; i < v; i++)
            {
                toReturn+="[i"+i+"]";
            }
            return toReturn;
        }

        private static string CheckIfSensorNeedsManaging()
        {
            string text = string.Empty;

            text = string.Concat(text, GetOperationToTargetProperty(2, 0));

            //if (mapperType.IsSubclassOf(typeof(CollectionMapper)))
            //{
            //    //Checking if something changed
            //    text = string.Concat(text, "" +
            //        string.Format("" +
            //            "{0}if({3} > isIndexActive.Count)\n" +
            //            "{0}{{\n" +
            //                "{1}for(int i = isIndexActive.Count; i < {3}; i++)\n" +
            //                "{1}{{\n" +
            //                    "{2}indicies.Add({5});\n" +
            //                    "{2}isIndexActive.Add(true);\n" +
            //                    "{2}values.Add(new List<{4}>());\n" +
            //                "{1}}}\n" +
            //            "{0}}}\n" +
            //            "{0}else if({3} < isIndexActive.Count)\n" +
            //            "{0}{{\n" +
            //                "{1}for(int i = {3}; i < isIndexActive.Count; i++)\n" +
            //                "{1}{{\n" +
            //                    "{2}indicies.RemoveAt(isIndexActive.Count - 1);\n" +
            //                    "{2}isIndexActive.RemoveAt(isIndexActive.Count - 1);\n" +
            //                    "{2}values.RemoveAt(isIndexActive.Count - 1);\n" +
            //                "{1}}}\n" +
            //            "{0}}}\n", GetTabs(2), GetTabs(3), GetTabs(4), GetCollectionSize(), TypeNameOrAlias(((CollectionMapper)mapper).ElementType(finalType)), GetCollectionIndex()));

            //    if (!((CollectionMapper)mapper).ElementType(finalType).IsPrimitive)
            //    {
            //        //The single element can be null
            //        text = string.Concat(text, "" +
            //            string.Format("" +
            //            "{0}for(int i = 0; i < values.Count; i++)\n" +
            //            "{0}{{\n" +
            //                "{1}if({3} == null && isIndexActive[i])\n" +
            //                "{1}{{\n" +
            //                    "{2}isIndexActive[i] = false;\n" +
            //                    "{2}values[i].Clear();\n" +
            //                "{1}}}\n" +
            //                "{1}else if({3} != null && !isIndexActive[i])\n" +
            //                "{1}{{\n" +
            //                    "{2}isIndexActive[i] = true;\n" +
            //                "{1}}}\n" +
            //            "{0}}}\n", GetTabs(2), GetTabs(3), GetTabs(4), GetCollectionElement()));
            //    }
            //}

            return text;
        }

        private static string GetOperationToTargetProperty(int baseOfTabs, int positionInHierarchy)
        {
            //Debug.Log(positionInHierarchy + " " + finalType.Name);
            string text = string.Empty;
            if (positionInHierarchy == iDataMapperTypes.Count)
            {
                text = string.Concat(text, "" +
                    string.Format("{3}{0} {1}{2} = {4}{5}[i_{5}];{6}", TypeNameOrAlias(finalType), propertyHierarchyNames[positionInHierarchy - 1], positionInHierarchy, GetTabs(baseOfTabs), propertyHierarchyNames[positionInHierarchy - 1], positionInHierarchy - 1, Environment.NewLine));
                if (finalType.IsPrimitive)
                {
                    text = string.Concat(text, "" +
                        string.Format("" +
                        "{0}", UpdateSensorValues(baseOfTabs)));
                }
                else
                {
                    text = string.Concat(text, "" +
                        string.Format("" +
                        "{0}if({2}{3} == null){6}" +
                        "{0}{{{6}" +
                            "{1}values{4}.Clear();{6}" +
                            "{1}continue;{6}" +
                        "{0}}}{6}" +
                        "{0}else{6}" +
                        "{0}{{{6}" +
                        "{5}" +
                        "{0}}}{6}", GetTabs(baseOfTabs), GetTabs(baseOfTabs + 1), propertyHierarchyNames[positionInHierarchy - 1], positionInHierarchy, GetCollectionElement(positionInHierarchy), UpdateSensorValues(baseOfTabs + 1,positionInHierarchy), Environment.NewLine)); ;
                }
                return text;
            }
            else if (positionInHierarchy == 0 || iDataMapperTypes[positionInHierarchy - 1] == null)
            {
                text = string.Concat(text, "" +
                    string.Format("{3}{0} {1}{2} = {4}{5}", propertyHierarchyTypeNames[positionInHierarchy], propertyHierarchyNames[positionInHierarchy], positionInHierarchy, GetTabs(baseOfTabs), positionInHierarchy == 0 ? "gameObject" : propertyHierarchyNames[positionInHierarchy - 1], positionInHierarchy == 0 ? "" : ""+(positionInHierarchy - 1)));

            }
            else 
            {
                text = string.Concat(text, "" +
                    string.Format("" +
                    "{3}if({4}{5}[i_{6}] == null){7}" + 
                    "{3}{{{7}" +
                        "{8}values{9}.Clear();{7}" +
                        "{8}{10};{7}" +
                    "{3}}}{7}" +
                    "{3}{0} {1}{2} = {4}{5}[i_{6}]", propertyHierarchyTypeNames[positionInHierarchy], propertyHierarchyNames[positionInHierarchy], positionInHierarchy, GetTabs(baseOfTabs), positionInHierarchy == 0 ? "gameObject" : propertyHierarchyNames[positionInHierarchy - 1], positionInHierarchy == 0 ? "" : "" + (positionInHierarchy - 1), positionInHierarchy - 1,Environment.NewLine, GetTabs(baseOfTabs+1), GetCollectionElement(positionInHierarchy), GetNumberOfCollectionMapperBeforePosition(positionInHierarchy) == 0 ? "return":"continue"));
            }

            if ((positionInHierarchy == 0 || propertyHierarchyTypeNames[positionInHierarchy - 1].Equals("GameObject")) && arePropertiesComponent[positionInHierarchy])
            {
                text = string.Concat(text, "" +
                    string.Format(".GetComponent<{0}>();{1}", propertyHierarchyTypeNames[positionInHierarchy], Environment.NewLine));
            }
            else
            {
                text = string.Concat(text, "" +
                    string.Format(".{0};{1}", propertyHierarchyNames[positionInHierarchy], Environment.NewLine));
            }

            if(iDataMapperTypes[positionInHierarchy] != null && positionInHierarchy != 0 )//&& positionInHierarchy != propertyHierarchyNames.Count - 1)
            {
                //Debug.Log(1);
                text = string.Concat(text, "" +
                    string.Format("" +
                    "{0}if({2}{3} == null){7}" +
                    "{0}{{{7}" +
                        "{1}values{4}.Clear();{7}" +
                        "{1}{9};{7}" +
                    "{0}}}{7}" +
                    "{0}else if({5} > values{4}.Count){7}" +
                    "{0}{{{7}" +
                        "{1}for(int i = values{4}.Count; i < {5}; i++){7}" +
                        "{1}{{{7}" +
                            "{8}values{4}.Add(new {6}());{7}" +
                        "{1}}}{7}" +
                    "{0}}}{7}" +
                    "{0}else if({5} < values{4}.Count){7}" +
                    "{0}{{{7}" +
                        "{1}for(int i = {5}; i < values{4}.Count; i++){7}" +
                        "{1}{{{7}" +
                            "{8}values{4}.RemoveAt(values{4}.Count - 1);{7}" +
                        "{1}}}{7}" +
                    "{0}}}{7}", GetTabs(baseOfTabs), GetTabs(baseOfTabs + 1), propertyHierarchyNames[positionInHierarchy], positionInHierarchy, GetCollectionElement(positionInHierarchy), GetCollectionSize(iDataMapperTypes[positionInHierarchy], string.Format("{0}{1}", propertyHierarchyNames[positionInHierarchy], positionInHierarchy)), GetRecursiveString("List<{0}>", numberOfCollectionMappers - GetNumberOfCollectionMapperBeforePosition(positionInHierarchy)), Environment.NewLine, GetTabs(baseOfTabs + 2), GetNumberOfCollectionMapperBeforePosition(positionInHierarchy) == 0?"return":"continue")); 

            }

            if (positionInHierarchy == propertyHierarchyNames.Count - 1 && iDataMapperTypes[positionInHierarchy]==null)
            {
                //Debug.Log(2);

                if (arePropertiesPrimitive[positionInHierarchy])
                {
                    text = string.Concat(text, "" +
                        string.Format("" +
                        "{0}", UpdateSensorValues(baseOfTabs)));
                }
                else
                {
                    text = string.Concat(text, "" +
                        string.Format("" +
                        "{0}if({2}{3} == null){6}" +
                        "{0}{{{6}" +
                            "{1}values{4}.Clear();{6}" +
                            "{1}{7};{6}" +
                        "{0}}}{6}" +
                        "{0}else{6}" +
                        "{0}{{{6}" +
                        "{5}" +
                        "{0}}}{6}", GetTabs(baseOfTabs), GetTabs(baseOfTabs + 1), propertyHierarchyNames[positionInHierarchy], positionInHierarchy, GetCollectionElement(positionInHierarchy), UpdateSensorValues(baseOfTabs + 1), Environment.NewLine, GetNumberOfCollectionMapperBeforePosition(positionInHierarchy)==0 ? "return" : "continue")); ;
                }
            }
            if (iDataMapperTypes[positionInHierarchy] != null)
            {
                text = string.Concat(text, "" +
                    string.Format("" +
                    "{0}for(int i_{1} = 0; i_{1} < {2}; i_{1}++){3}" +
                    "{0}{{{3}" +
                        "{4}" +
                    "{0}}}{3}", GetTabs(baseOfTabs), positionInHierarchy, GetCollectionSize(iDataMapperTypes[positionInHierarchy], string.Format("{0}{1}", propertyHierarchyNames[positionInHierarchy], positionInHierarchy)), Environment.NewLine, GetOperationToTargetProperty(baseOfTabs + 1, positionInHierarchy + 1)));
            }
            else if(positionInHierarchy<propertyHierarchyNames.Count-1)
            {
                text = string.Concat(text, GetOperationToTargetProperty(baseOfTabs, positionInHierarchy + 1));
            }

            return text;
        }


        private static string GetLists(int n, string type)
        {
            string lists = "";
            string close = "";
            for(int i=0; i < n; i++)
            {
                lists += "List<";
                close += ">";
            }
            return lists+type+close;
        }

        private static int GetNumberOfCollectionMapperBeforePosition(int position)
        {
            int count = 0;

            for(int i = 0; i < position; i++)
            {
                if (iDataMapperTypes[i] != null)
                {
                    count++;
                }
            }

            return count;
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

        private static string GetCollectionElement(int positionInHierarchy)
        {
            string text = string.Empty;

            int number = GetNumberOfCollectionMapperBeforePosition(positionInHierarchy);
            int count = 0;
            for (int i = 0; i < positionInHierarchy; i++)
            {
                if (iDataMapperTypes[i] != null)
                {
                    text = string.Concat(text, string.Format("[i_{0}]", i));
                    count++;

                    if (count >= number) break;
                }
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

        private static string GetCollectionSize(Type mapper, string propertyName)
        {
            //Debug.Log(mapper+" "+propertyName);
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

        private static string UpdateSensorValues(int baseTabs, int finalId = -1)
        {
            if(finalId== -1)
            {
                finalId=propertyHierarchyNames.Count - 1;
            }
            string text = string.Empty;

            int maxValue = currentSensorConfiguration.PropertyFeaturesList.Find(x => x.property.Equals(currentPropertyHierarchy)).windowWidth;

            text = string.Concat(text, "" +
                string.Format("" +
                    "{0}if (values{5}.Count == {4}){6}" +
                    "{0}{{{6}" +
                        "{3}values{5}.RemoveAt(0);{6}" +
                    "{0}}}{6}" +
                    "{0}values{5}.Add({1}{2});{6}", GetTabs(baseTabs), propertyHierarchyNames[propertyHierarchyNames.Count - 1], finalId, GetTabs(baseTabs + 2), maxValue, GetCollectionElement(finalId), Environment.NewLine));

            ////Updating
            //if (mapperType.IsSubclassOf(typeof(BasicTypeMapper)))
            //{
            //    text = string.Concat(text, "" +
            //        string.Format("" +
            //            "{0}if (values.Count == {4})\n" +
            //            "{0}{{\n" +
            //                "{3}values.RemoveAt(0);\n" +
            //            "{0}}}\n" +
            //            "{0}values.Add({1}{2});\n", GetTabs(2), propertyHierarchyNames[propertyHierarchyNames.Count - 1], propertyHierarchyNames.Count - 1, GetTabs(3), maxValue));

            //}
            //else if (mapperType.IsSubclassOf(typeof(CollectionMapper)))
            //{
            //    text = string.Concat(text, "" +
            //        string.Format("" +
            //        "{0}for(int i = 0; i < values.Count; i++)\n" +
            //        "{0}{{\n" +
            //            "{1}if (values[i].Count == {4})\n" +
            //            "{1}{{\n" +
            //                "{2}values[i].RemoveAt(0);\n" +
            //            "{1}}}\n" +
            //            "{1}{3}" +
            //        "{0}}}\n", GetTabs(2), GetTabs(3), GetTabs(4), UpdateSensorValue(), maxValue));
            //}

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
                //Debug.Log("Property: " + currentProperty);
                currentObjectValue = RetrieveProperty(currentObjectValue, currentProperty, currentType, out currentType);
                //Debug.Log("Type: " + currentType);
                IDataMapper tempMapper = MapperManager.GetMapper(currentType);
                if (tempMapper != null)
                {
                    mapper = tempMapper;
                }
                //Debug.Log("Mapper: " + mapper);
                //Debug.Log("Current Object Value= " + (currentObjectValue == null? "NULL":currentObjectValue.GetType()) );


                bool returnFalse = currentObjectValue == null && !ReachPropertyByReflectionByType(property, currentType, out finalType, out mapper);
                Type tempType = currentType;
                if (tempMapper != null && mapper is CollectionMapper collectionMapper)
                {
                    //Debug.Log("Found a CollectionMapper");
                    iDataMapperTypes.Add(mapper.GetType());
                    numberOfCollectionMappers++;
                    currentType = collectionMapper.ElementType(currentType);
                    //Debug.Log("The new currentType is " + currentType.Name);
                }
                else
                {
                    iDataMapperTypes.Add(null);
                }
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
                    propertyHierarchyNames.Add(currentProperty);
                    propertyHierarchyTypeNames.Add(TypeNameOrAlias(tempType));
                    propertyHierarchyTypeNamespaces.Add(tempType.Namespace);
                    arePropertiesComponent.Add(tempType.IsSubclassOf(typeof(Component)));
                    arePropertiesPrimitive.Add(tempType.IsPrimitive);
                    property.RemoveAt(0);
                    if (property.Count > 0 && currentType.Name == property[0])
                    {
                        property.RemoveAt(0);
                    }
                    if (property.Count == 0)
                    {
                        finalType = currentType;
                        if(mapper is CollectionMapper collectionMapper2)
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
        private static bool ReachPropertyByReflectionByType(MyListString property, Type currentType, out Type finalType, out IDataMapper mapper)
        {
            finalType = null;
            mapper = null;
            string currentProperty = string.Empty;
            while (property.Count > 0)
            {
                //Debug.Log(property);
                currentProperty = property[0];
                currentType = RetrievePropertyByType(currentProperty, currentType);
                //Debug.Log("Current Type= " + currentType);

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
                        //Debug.Log("Found a CollectionMapper");
                        iDataMapperTypes.Add(mapper.GetType());
                        numberOfCollectionMappers++;
                        currentType = collectionMapper.ElementType(currentType);
                        //Debug.Log("The new currentType is " + currentType.Name);
                    }
                    else
                    {
                        iDataMapperTypes.Add(null);
                    }
                    propertyHierarchyNames.Add(currentProperty);
                    propertyHierarchyTypeNames.Add(TypeNameOrAlias(tempType));
                    propertyHierarchyTypeNamespaces.Add(tempType.Namespace);
                    arePropertiesComponent.Add(tempType.IsSubclassOf(typeof(Component)));
                    arePropertiesPrimitive.Add(tempType.IsPrimitive);
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
                            mapper = MapperManager.GetMapper(collectionMapper2.ElementType(finalType));
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
