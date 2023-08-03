using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ThinkEngine.it.unical.mat.objectsMapper.BrainsScripts;
using ThinkEngine.Mappers;
using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine
{
    [ExecuteAlways]
    public abstract class Brain : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        bool borning=true;
        internal enum SOLVER { Clingo, DLV2, Incremental_DLV2 }
        internal string executorName;
        internal string SolverName
        {
            get
            {
                return SolversChecker.GetSolverName(solverEnum);
            }
        }
        
        private string _fileExtension;
        internal string FileExtension
        {
            get
            {
                if (_fileExtension == null)
                {
                    FindCurrentParadigm();
                    if (_fileExtension == null)
                    {
                        return "";
                    }
                }
                return _fileExtension;
            }
        }
        protected abstract HashSet<string> SupportedFileExtensions { get; }
        public bool enableBrain = true;
        public bool debug = true;
        internal bool incremental = false;
        [SerializeField, HideInInspector]
        internal bool maintainInputFile;
        [SerializeField, HideInInspector]
        protected List<string> _chosenSensorConfigurations;
        internal List<string> ChosenSensorConfigurations
        {
            get
            {
                if (_chosenSensorConfigurations == null)
                {
                    _chosenSensorConfigurations = new List<string>();
                }
                return _chosenSensorConfigurations;
            }
        }
        [SerializeField, HideInInspector]
        internal string AIFilesPath = Utility.StreamingAssetsContent;
        [SerializeField]
        internal List<string> AIFilesPrefix;
        
        [SerializeField, HideInInspector]
        internal bool prefabBrain;
        [SerializeField, HideInInspector]
        internal SOLVER solverEnum;
        [SerializeField, HideInInspector]
        protected string _executeReasonerOn;
        internal string ExecuteReasonerOn
        {
            get
            {
                if (_executeReasonerOn == null)
                {
                    _executeReasonerOn = "";
                }
                return _executeReasonerOn;
            }
            set
            {
                _executeReasonerOn = value;
            }
        }
        [SerializeField, HideInInspector]
        internal bool specificAIFile;
        [SerializeField, HideInInspector]
        internal bool globalAIFile;
        protected object triggerClass;
        internal readonly object toLock = new object();
        internal string sensorsMapping;
        internal bool sensorsConfigurationsChanged;
        protected string originalName;
        internal MethodInfo reasonerMethod;
        internal bool missingData;
        protected Thread executionThread;
        internal Executor executor;
        internal bool solverWaiting;
        public List<string> solver_options;
        internal string brainName;
        internal string whoAmIFact;

        void OnEnable()
        {
            originalName = gameObject.name;
            if (borning)
            {
                solverEnum = SolversChecker.AvailableSolvers()[0];
            }
            borning = false;
        }
        void Awake()
        {
            AIFilesPath = Utility.StreamingAssetsContent;
        }

        protected virtual void Start()
        {
            Utility.LoadPrefabs();
            if (Application.isPlaying && enableBrain)
            {
                triggerClass = Utility.TriggerClass;
                StartCoroutine(Init());
                whoAmIFact = "currentBrainID("+GetInstanceID()+")."+Environment.NewLine;
            }
        }

        private void FindCurrentParadigm()
        {
            if (AIFilesPrefix == null)
            {
                return;
            }
            _fileExtension = "";
            foreach (string fileName in Directory.GetFiles(Utility.StreamingAssetsContent))
            {
                string actualFileName = fileName.Substring(fileName.LastIndexOf(Utility.slash) + 1);
                foreach (string prefix in AIFilesPrefix)
                {
                    if (actualFileName.StartsWith(prefix))
                    {
                        string extension = actualFileName.Substring(actualFileName.LastIndexOf(".") + 1);
                        if (FileExtension.Equals("") && SupportedFileExtensions.Contains(extension))
                        {
                            _fileExtension = extension;
                        }
                        else if (!extension.Equals(FileExtension) && SupportedFileExtensions.Contains(extension))
                        {
                            Debug.LogError("Multiple paradigms encoding found. You should either use " + FileExtension + " or " + extension + " for " + AIFilesPrefix);
                        }
                    }
                }
            }

            if (FileExtension.Equals(""))
            {
                _fileExtension = null;
            }
        }

        protected virtual void Update()
        {
            if (Application.isPlaying && reasonerMethod == null)
            {
                lock (toLock)
                {
                    if (!SomeConfigurationAvailable())
                    {
                        missingData = true;
                        return;
                    }
                    if (solverWaiting)
                    {
                        solverWaiting = false;
                        Monitor.Pulse(toLock);
                    }
                }
            }
        }
        internal void RemoveNullSensorConfigurations()
        {
            ChosenSensorConfigurations.RemoveAll(x => !Utility.SensorsManager.ExistsConfigurationWithName(x));
        }
        protected void PrepareSensors()
        {
            Utility.SensorsManager.RegisterBrainsSensorConfigurations(this, ChosenSensorConfigurations);
        }
        protected IEnumerator PulseOn()
        {
            while (true)
            {

                yield return new WaitUntil(() => solverWaiting && SomeConfigurationAvailable() && (bool)reasonerMethod.Invoke(triggerClass, null));
                lock (toLock)
                {
                    solverWaiting = false;
                    Monitor.Pulse(toLock);
                }
            }
        }
        
        internal string GetAIFileTemplatePath() {
            string aiFileName = gameObject.name;
            // if(AIFilesPrefix != null && AIFilesPrefix.Count > 0) {
            //     aiFileName = string.Join("-", AIFilesPrefix);
            // }
            var brainIndex = GetComponents(GetType()).ToList().IndexOf(this);
            

            aiFileName += GetType().Name + brainIndex + "Template.asp";
            var aiFileTemplatePath = Path.Combine(Utility.TemplatesFolder, aiFileName);
            return aiFileTemplatePath;
        }

        internal virtual void GenerateFile()
        {
            string sensorsAsASP;
            
            if (!Directory.Exists(Utility.TemplatesFolder)) 
                Directory.CreateDirectory(Utility.TemplatesFolder);

            using (StreamWriter fs = new StreamWriter(GetAIFileTemplatePath()))
            {
                fs.Write("%For runtime instantiated GameObject, only the prefab mapping is provided. Use that one substituting the gameobject name accordingly.\n %Sensors.\n");
                HashSet<string> seenSensorConfNames = new HashSet<string>();

                foreach (SensorConfiguration sensorConf in Utility.SensorsManager.GetConfigurations(ChosenSensorConfigurations))
                {
                    if (seenSensorConfNames.Contains(sensorConf.ConfigurationName))
                    {
                        continue;
                    }
                    seenSensorConfNames.Add(sensorConf.ConfigurationName);
                    foreach (PropertyFeatures features in sensorConf.PropertyFeaturesList)
                    {
                        //Debug.Log(property);
                        sensorsAsASP = MapperManager.GetASPTemplate(features.PropertyAlias, sensorConf.gameObject, features.property, true);
                        //Debug.Log(sensorsAsASP);
                        fs.Write(ActualSensorEncoding(sensorsAsASP));
                    }
                }
                fs.Write(SpecificFileParts());
            }
        }

        protected abstract string SpecificFileParts();
        internal abstract string ActualSensorEncoding(string sensorsAsASP);

        protected virtual IEnumerator Init()
        {
            if (specificAIFile && originalName.Equals(gameObject.name))
            {
                yield return new WaitUntil(() => !originalName.Equals(gameObject.name));
            }
            brainName=gameObject.name;
            PrepareSensors();
            if (!SomeConfigurationAvailable())
            {
                missingData = true;
            }
            if (!ExecuteReasonerOn.Equals("When Sensors are ready"))
            {
                reasonerMethod = Utility.GetTriggerMethod(ExecuteReasonerOn);
                if (reasonerMethod != null)
                {
                    StartCoroutine(PulseOn());
                }
            }
        }
        protected virtual bool SomeConfigurationAvailable()
        {
            return Utility.SensorsManager.IsSomeActiveInScene(ChosenSensorConfigurations);
        }
        void OnApplicationQuit()
        {
            if (executor != null)
            {
                executor.reason = false;
                Executor.die = true;
                lock (toLock)
                {
                    Monitor.Pulse(toLock);
                }
                Thread.Sleep(500);
            }
        }
    }

}
