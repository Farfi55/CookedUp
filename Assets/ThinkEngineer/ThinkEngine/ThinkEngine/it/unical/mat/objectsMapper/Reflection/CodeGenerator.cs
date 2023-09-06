using it.unical.mat.embasp.languages;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ThinkEngine.Mappers.BaseMappers;
using UnityEditor;
using UnityEngine;
using System;

namespace ThinkEngine.ScriptGeneration
{
    internal class CodeGenerator
    {
        // Paths
        private static readonly string rootPath = string.Format("{0}", Path.Combine(Application.dataPath, "Scripts"));
        private static readonly string generatedCodeRelativePath = Path.Combine("Assets", "Scripts", "GeneratedCode");
        private static readonly string generatedCodePath = string.Format(Path.Combine("{0}", "GeneratedCode"), rootPath);

        private CodeGenerator() { }

        internal static void GenerateCode(SensorConfiguration sensorConfiguration)
        {
            if (sensorConfiguration.ConfigurationName.Equals(string.Empty))
            {
                Debug.LogError("SensorConfiguration name can't be empty!");
                return;
            }
            foreach (string fileName in sensorConfiguration.generatedScripts)
            {
                DeleteGeneratedScript(fileName);
            }
            sensorConfiguration.generatedScripts.Clear();

            List<MyListString> toRemove = new List<MyListString>();

            foreach (MyListString currentPropertyHierarchy in sensorConfiguration.ToMapProperties)
            {
                PropertyReflectionData reflectionData = new PropertyReflectionData(currentPropertyHierarchy, sensorConfiguration.gameObject, sensorConfiguration);

                if (!ReflectionUtility.PopulateReflectionData(reflectionData))
                {
                    toRemove.Add(currentPropertyHierarchy);
                    continue;
                }

                //Debug.Log(reflectionData);

                string content = TextGenerationUtility.GenerateSensorScript(reflectionData);
                string fileName = reflectionData.PropertyFeatures.PropertyAlias + ".cs";
                string path = Path.Combine(generatedCodePath, fileName);

                if (!Directory.Exists(generatedCodePath)) Directory.CreateDirectory(generatedCodePath);

                //Debug.Log(content);
                File.WriteAllText(path, content);
                sensorConfiguration.generatedScripts.Add(fileName);
            }

            // Refresh the unity asset database
            bool changed = false;
            foreach (MyListString p in toRemove)
            {

                Debug.Log("TE: removing " + p + "from "+sensorConfiguration.ConfigurationName+" in "+sensorConfiguration.gameObject.name);
                sensorConfiguration.ToggleProperty(p, false);
                changed = true;
            }
            if (changed)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(sensorConfiguration);
#endif
            }
        }

#if UNITY_EDITOR
        internal static void AttachSensorsScripts(SensorConfiguration sensorConfiguration)
        {
            /*foreach (string monoScript in AssetDatabase.FindAssets("t:MonoScript", new string[] { "Assets/Scripts/GeneratedCode" }))
            {
                Debug.Log("Found this asset: "+monoScript);
                Debug.Log(AssetDatabase.GUIDToAssetPath(monoScript));
            }*/
            sensorConfiguration._serializableSensorsTypes.Clear();
            foreach (PropertyFeatures pF in sensorConfiguration.PropertyFeaturesList)
            {
                MonoScript retrieved = AssetDatabase.LoadAssetAtPath(Path.Combine("Assets", "Scripts", "GeneratedCode", pF.PropertyAlias + ".cs"), typeof(MonoScript)) as MonoScript;
                if (retrieved != null)
                {
                    sensorConfiguration._serializableSensorsTypes.Add(new SerializableSensorType(retrieved));
                }
            }
        }
#endif

        internal static void Rename(string oldAlias, string newAlias, SensorConfiguration sensorConfiguration)
        {
            foreach (string path in sensorConfiguration.generatedScripts)
            {
                string oldPath = Path.Combine(generatedCodePath, path);
                if (File.Exists(oldPath))
                {
                    if (path.EndsWith(oldAlias + ".cs"))
                    {
                        string newPath = oldPath.Replace(oldAlias, newAlias);
                        File.Move(path, newPath);
                        if (File.Exists(path + ".meta"))
                        {
                            File.Delete(path + ".meta");
                        }
                    }
                }
                else
                {
                    sensorConfiguration.GenerateScripts();
                }
            }
        }

        internal static void RemoveUseless(MyListString property, SensorConfiguration configuration)
        {
            string alias = configuration.PropertyFeaturesList.Find(x => x.property.Equals(property)).PropertyAlias;
            if (alias != null)
            {
                string toDelete = configuration.generatedScripts.Find(x => x.Contains(alias));
                DeleteGeneratedScript(toDelete);
                configuration.generatedScripts.Remove(toDelete);
            }
        }

        private static void DeleteGeneratedScript(string toDelete)
        {
            if (toDelete == null) {
                return;

            }
            string path = Path.Combine(generatedCodePath, toDelete);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (File.Exists(path + ".meta"))
            {
                File.Delete(path + ".meta");

            }
        }
    }
}
