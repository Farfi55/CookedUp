using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ThinkEngine.it.unical.mat.objectsMapper.BrainsScripts;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThinkEngine
{


    public static class Utility
    {
        public static BindingFlags BindingAttr = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
        private static List<string> _triggerMethodsToShow;
        internal static ScriptableObject _triggerClass;
        private static GameObject _hiddenGameObject;
        private static SensorsManager _sensorsManager;
        private static ActuatorsManager _actuatorsManager;
        internal static string slash = Path.DirectorySeparatorChar + "";
        internal static string ThinkEngineBaseFolder = Path.Combine(".", "Assets", "ThinkEngineer", "ThinkEngine");
        private static string _triggerClassPath = Path.Combine(ThinkEngineBaseFolder,"Scripts", "ThinkEngineTrigger.cs");
        private static string actualTriggerClassPath = "";
        internal static bool prefabsLoaded = false;
        private static MethodInfo[] _triggerMethods;
        internal static string RunnableExtension = Environment.OSVersion.Platform == PlatformID.Win32NT ? ".exe" : "";
        internal static string StreamingAssetsContent = Path.Combine(Application.streamingAssetsPath, "ThinkEngineer", "ThinkEngine");
        internal static string TemplatesFolder = Path.Combine(ThinkEngineBaseFolder, "Templates");
        private static MethodInfo[] TriggerMethods
        {
            get
            {
                if (_triggerMethods == null)
                {
                    try
                    {
                        _triggerMethods = TriggerClass.GetType().GetMethods(BindingAttr);
                    }
                    catch
                    {
                        return new MethodInfo[0];
                    }
                }
                return _triggerMethods;
            }
        }
        #region Properties

        public static bool UpdatingSensors
        {
            get
            {
                return Executor.Reading(); ;
            }
        }
        internal static List<string> TriggerMethodsToShow
        {
            get
            {
                CheckTriggerClass();
                _triggerMethodsToShow = FindMethodsToShow();
                return _triggerMethodsToShow;
            }
        }
        internal static ScriptableObject TriggerClass
        {
            get
            {
                CheckTriggerClass();
                if (_triggerClass == null)
                {
#if UNITY_EDITOR
                    if (!EditorApplication.isCompiling)
                    {
                        _triggerClass = ScriptableObject.CreateInstance("ThinkEngineTrigger");

                    }
#endif
#if !UNITY_EDITOR
                _triggerClass = ScriptableObject.CreateInstance("Trigger");
#endif
                }
#if UNITY_EDITOR
                MonoScript sourceScriptAsset = MonoScript.FromScriptableObject(_triggerClass);
                actualTriggerClassPath = AssetDatabase.GetAssetPath(sourceScriptAsset);
#endif
                return _triggerClass;
            }
        }
        internal static GameObject HiddenGameObject
        {
            get
            {
                if (_hiddenGameObject == null)
                {
                    _hiddenGameObject = GameObject.Find("ThinkEngineUtility");
                    if (_hiddenGameObject == null)
                    {
                        _hiddenGameObject = new GameObject("ThinkEngineUtility");
                        //_hiddenGameObject.hideFlags = HideFlags.HideInHierarchy & HideFlags.HideInInspector;
                    }
                }
                return _hiddenGameObject;
            }
        }
        internal static bool ManagersDestroyed
        {
            get
            {
                return SensorsManager.destroyed || ActuatorsManager.destroyed;
            }
        }
        internal static SensorsManager SensorsManager
        {
            get
            {
                if (_sensorsManager == null)
                {
                    _sensorsManager = HiddenGameObject.GetComponent<SensorsManager>();
                    if (_sensorsManager == null)
                    {
                        _sensorsManager = HiddenGameObject.AddComponent<SensorsManager>();
                    }
                }
                return _sensorsManager;
            }
        }
        internal static ActuatorsManager ActuatorsManager
        {
            get
            {
                if (_actuatorsManager == null)
                {
                    _actuatorsManager = HiddenGameObject.GetComponent<ActuatorsManager>();
                    if (_actuatorsManager == null)
                    {
                        _actuatorsManager = HiddenGameObject.AddComponent<ActuatorsManager>();
                    }
                }
                return _actuatorsManager;
            }
        }
        internal static string TriggerClassPath
        {
            get
            {
                CheckTriggerClass();
                if (!actualTriggerClassPath.Equals(""))
                {
                    return actualTriggerClassPath;
                }
                return _triggerClassPath;
            }
        }
        #endregion
        private static List<string> FindMethodsToShow()
        {
            List<string> toReturn = new List<string>();
            foreach (MethodInfo mI in TriggerMethods)
            {
                if (mI.ReturnType == typeof(bool) && mI.GetParameters().Length == 0)
                {
                    toReturn.Add(mI.Name);
                }
            }
            return toReturn;
        }
        internal static int GetTriggerMethodIndex(string name)
        {
            int index = TriggerMethodsToShow.IndexOf(name);
            if (index != -1)
            {
                return index;
            }
            return TriggerMethodsToShow.Count;
        }
        internal static MethodInfo GetTriggerMethod(int chosenMethod)
        {
            if (chosenMethod > TriggerMethods.Length || chosenMethod < 0)
            {
                return null;
            }
            return TriggerMethods[chosenMethod];
        }
        internal static MethodInfo GetTriggerMethod(string chosenMethod)
        {
            if (!TriggerMethodsToShow.Contains(chosenMethod))
            {
                return null;
            }
            return TriggerMethods[TriggerMethodsToShow.IndexOf(chosenMethod)];
        }
        internal static void CheckTriggerClass()
        {
#if UNITY_EDITOR
            if (!actualTriggerClassPath.Equals("") && File.Exists(actualTriggerClassPath) && actualTriggerClassPath.EndsWith("ThinkEngineTrigger.cs"))
            {
                return;
            }
            if (Directory.Exists(Path.Combine(ThinkEngineBaseFolder, "Scripts")) && File.Exists(_triggerClassPath))
            {
                return;
            }
            var o = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                     from type in assembly.GetTypes()
                     where type.Name == "ThinkEngineTrigger"
                     select type).FirstOrDefault();
            if (o != null)
            {
                return;
            }
            Directory.CreateDirectory(Path.Combine(ThinkEngineBaseFolder, "Scripts"));
            CreateTriggerScript();
#endif
        }
        private static void CreateTriggerScript()
        {
#if UNITY_EDITOR
            using (FileStream fs = File.Create(_triggerClassPath))
            {
                string triggerClassContent = "using System;"+Environment.NewLine;
                triggerClassContent += "using UnityEngine;" + Environment.NewLine;
                triggerClassContent += @"// every method of this class without parameters and that returns a bool value can be used to trigger the reasoner." + Environment.NewLine;
                triggerClassContent += "namespace ThinkEngine \n{\n";
                triggerClassContent += "\t public class ThinkEngineTrigger:ScriptableObject\n{\n\n";
                triggerClassContent += "\t}\n}";
                byte[] info = new UTF8Encoding(true).GetBytes(triggerClassContent);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }

            AssetDatabase.Refresh();
#endif
        }
        internal static void LoadPrefabs()
        {
            if (!prefabsLoaded)
            {
                Resources.LoadAll("");
                prefabsLoaded = true;
            }
        }
    }
}