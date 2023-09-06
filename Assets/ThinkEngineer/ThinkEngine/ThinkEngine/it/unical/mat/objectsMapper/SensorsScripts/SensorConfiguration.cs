using System;
using System.Collections.Generic;
using System.Reflection;
using ThinkEngine.Mappers;
using UnityEditor;
using UnityEditor.Compilation;
using ThinkEngine.ScriptGeneration;
using UnityEngine;
using System.IO;

namespace ThinkEngine
{
    [ExecuteInEditMode, Serializable]
    public class SensorConfiguration : AbstractConfiguration//, ISerializationCallbackReceiver
    {
        public bool isInvariant;
        public bool isFixedSize;

        //GMDG
        //This array contains the types of the sensor assiated with "this" SensorConfiguration

        [SerializeField]
        internal List<SerializableSensorType> _serializableSensorsTypes = new List<SerializableSensorType>();
        [SerializeField,HideInInspector]
        internal bool recompile;
        [SerializeField, HideInInspector]
        internal bool teRecompile;
        private bool forceRecompile;
        [SerializeField]
        internal List<string> generatedScripts= new List<string>();
        private List<Sensor> _sensorsInstances = new List<Sensor>();

        internal static SensorConfiguration _instance;
        internal static SensorConfiguration Instance
        {
            get
            {
                if (_instance == null || !_instance.enabled)
                {
                    foreach(SensorConfiguration s in FindObjectsOfType<SensorConfiguration>())
                    {
                        if(s != null && s.enabled)
                        {
                            _instance = s;
                        }
                    }
                }
                return _instance;
            }
        }

        void Awake()
        {
            if(Application.isPlaying)
            {
#if UNITY_EDITOR
                CodeGenerator.AttachSensorsScripts(this);
#endif
                foreach (SerializableSensorType serializableSensorType in _serializableSensorsTypes)
                {
 //                   _sensorsInstances.Add((Sensor)serializableSensorType.ScriptType.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null));
                    _sensorsInstances.Add((Sensor)Activator.CreateInstance(serializableSensorType.ScriptType));

                }
                foreach (Sensor instance in _sensorsInstances)
                {
                    instance.Initialize(this);
                }
            }
        }

        internal override void PropertyAliasChanged(string oldAlias, string newAlias)
        {
#if UNITY_EDITOR
            CodeGenerator.Rename(oldAlias,newAlias,this);
            recompile = true;
#endif
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        static void Reload()
        {
            Debug.Log("Did Reload");
            if (Instance == null)
            {
                Debug.Log("Instance is null.");
                return;
            }
            if (Instance.teRecompile)
            {
                Debug.Log("TE recompiled.");
                Instance.teRecompile = false;
                return;
            }
            else
            {
                Debug.Log("Forcing TE recompile.");
                Instance.teRecompile=true;
                Instance.forceRecompile=true;
            }
        }
        static void Recompile()
        {
            Instance.teRecompile = true;
            Utility.LoadPrefabs();
            foreach (SensorConfiguration sensorConfiguration in Resources.FindObjectsOfTypeAll<SensorConfiguration>())
            {
                sensorConfiguration.GenerateScripts(false);
            }
            AssetDatabase.Refresh();
        }

        internal void GenerateScripts(bool _recompile=true)
        {
#if UNITY_EDITOR
            CodeGenerator.GenerateCode(this);
            recompile = _recompile;
            //CompilationPipeline.RequestScriptCompilation();
#endif
        }
        void Start()
        {
            Utility.LoadPrefabs();

        }
        void OnEnable()
        {
            if (Application.isPlaying)
            {
                SensorsManager.SubscribeSensors(_sensorsInstances, ConfigurationName);
            }
        }

        void OnDisable()
        {
            if (Application.isPlaying)
            {
                SensorsManager.UnsubscribeSensors(_sensorsInstances, ConfigurationName);
            }
        }

        void OnDestroy()
        {
            if (Application.isPlaying)
            {
                foreach(Sensor instance in _sensorsInstances)
                {
                    instance.Destroy();
                }
            }
        }
        //GMDG

        internal override string ConfigurationName
        {
            set
            {
                if (!Utility.SensorsManager.IsConfigurationNameValid(value, this))
                {
                    throw new Exception("The chosen configuration name cannot be used.");
                }
                string old = _configurationName;
                _configurationName = value;
                if (!old.Equals(_configurationName))
                {
                    SensorsManager.ConfigurationsChanged = true;
                }
            }
        }

        internal override void Clear()
        {
            base.Clear();
            _serializableSensorsTypes = new List<SerializableSensorType>(); // GMDG
        }
        internal override string GetAutoConfigurationName()
        {
            string name;
            string toAppend = "";
            int count = 0;
            do
            {
                name = ASPMapperHelper.AspFormat(gameObject.name) + "Sensor" + toAppend;
                toAppend += count;
                count++;
            }
            while (!Utility.SensorsManager.IsConfigurationNameValid(name, this));
            return name;
        }
        internal void SetOperationPerProperty(MyListString property, int operation)
        {
            if (!SavedProperties.Contains(property))
            {
                throw new Exception("Property not selected");
            }
            PropertyFeaturesList.Find(x => x.property.Equals(property)).operation = operation;
        }
        internal void SetSpecificValuePerProperty(MyListString property, string value)
        {
            if (!SavedProperties.Contains(property))
            {
                throw new Exception("Property not selected");
            }
            PropertyFeaturesList.Find(x => x.property.Equals(property)).specificValue = value;

        }

        internal void SetCounterPerProperty(MyListString actualProperty, int newCounter)
        {
            if (!SavedProperties.Contains(actualProperty))
            {
                throw new Exception("Property not selected");
            }
            PropertyFeaturesList.Find(x => x.property.Equals(actualProperty)).counter = newCounter;
        }

        internal override bool IsSensor()
        {
            return true;
        }
        
        protected override void PropertySelected(MyListString property)
        {
#if UNITY_EDITOR
            GenerateScripts();
#endif
        }
        protected override void PropertyDeleted(MyListString property) 
        {
#if UNITY_EDITOR
            if (ToMapProperties.Contains(property))
            {
                CodeGenerator.RemoveUseless(property, this);
            }
#endif
        }
#if UNITY_EDITOR
        void Update()
        {

            if (InEditMode()) 
            {
                if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text != "Inspector" && recompile)
                {
                    Debug.LogWarning("Compiling " + ConfigurationName + " generated scripts.");
                    recompile = false;
                    //CompilationPipeline.RequestScriptCompilation();
                    Instance.teRecompile = true;
                    AssetDatabase.Refresh();
                }
                else
                {
                    if (forceRecompile)
                    {
                        forceRecompile = false;
                        Recompile();
                    }
                }
            }

        }

        private bool InEditMode()
        {
            return !(EditorApplication.isPlaying || EditorApplication.isCompiling
                || EditorApplication.isPlayingOrWillChangePlaymode
                || EditorApplication.isUpdating);

        }
#endif
        /*
protected override void PropertyDeleted(MyListString property)
{

}

public void OnBeforeSerialize()
{
operationPerPropertyIndexes = new List<int>();
operationPerPropertyOperations = new List<int>();
specificValuePerPropertyIndexes = new List<int>();
specificValuePerPropertyValues = new List<string>();
foreach (int key in OperationPerProperty.Keys)
{
operationPerPropertyIndexes.Add(key);
operationPerPropertyOperations.Add(OperationPerProperty[key]);
}
foreach (int key in SpecificValuePerProperty.Keys)
{
specificValuePerPropertyIndexes.Add(key);
specificValuePerPropertyValues.Add(SpecificValuePerProperty[key]);
}
}

public void OnAfterDeserialize()
{
OperationPerProperty = new Dictionary<int, int>();
SpecificValuePerProperty = new Dictionary<int, string>();
for (int i = 0; i < operationPerPropertyIndexes.Count; i++)
{
OperationPerProperty.Add(operationPerPropertyIndexes[i], operationPerPropertyOperations[i]);
}
for (int i = 0; i < specificValuePerPropertyIndexes.Count; i++)
{
SpecificValuePerProperty.Add(specificValuePerPropertyIndexes[i], specificValuePerPropertyValues[i]);
}
}
*/
        internal override bool IsAValidName(string temporaryName)
        {
            return temporaryName.Equals(ConfigurationName) || Utility.SensorsManager.IsConfigurationNameValid(temporaryName, this);
        }

    }
}