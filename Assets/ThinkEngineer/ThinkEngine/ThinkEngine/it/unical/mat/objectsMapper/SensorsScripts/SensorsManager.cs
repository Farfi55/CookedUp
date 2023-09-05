using System;
using System.Collections;
using System.Reflection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using ThinkEngine.it.unical.mat.objectsMapper.BrainsScripts;
using UnityEngine;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
namespace ThinkEngine
{


    [ExecuteInEditMode]
    public class SensorsManager : MonoBehaviour
    {
        internal static Stopwatch watch = new Stopwatch();
        public int MIN_AVG_FPS;
        public int MIN_CURRENT_FPS;
        public int MAX_AVG_FPS;
        public int MAX_CURRENT_FPS;
        internal static int MAX_MS = 5;
        internal List<float> MOVING_AVG_FRAMES;
        private float currentFps;
        internal static int updatedSensors;
        internal static long MS;
        private static Dictionary<Brain, List<string>> _instantiatedSensors;
        internal static bool _configurationsChanged;
        internal float avgFps = 0;
        internal static float bestAvgFps = 0;
        internal static int frameCount = 0;
        internal static bool destroyed;
        internal static int _iteration=0;
        public static int iteration { get { return _iteration; } }
        private static Dictionary<Brain, List<string>> InstantiatedSensors
        {
            get
            {
                if (_instantiatedSensors == null)
                {
                    _instantiatedSensors = new Dictionary<Brain, List<string>>();
                }
                return _instantiatedSensors;
            }
        }
        internal bool IsConfigurationNameValid(string name, SensorConfiguration newSensorConfiguration)
        {
#if UNITY_EDITOR

            if (name.Equals(""))
            {
                return false;
            }
            foreach (SensorConfiguration configurationToCompare in Resources.FindObjectsOfTypeAll<SensorConfiguration>())
            {
                if (configurationToCompare == newSensorConfiguration)
                {
                    continue;
                }
                if (PrefabStageUtility.GetPrefabStage(newSensorConfiguration.gameObject) != null)
                {
                    GameObject toComparePrefab = PrefabUtility.GetCorrespondingObjectFromSource(configurationToCompare.gameObject);
                    if (toComparePrefab != null && newSensorConfiguration.gameObject.Equals(toComparePrefab))
                    {
                        continue;
                    }
                }
                if (configurationToCompare != null && configurationToCompare.ConfigurationName == name)
                {
                    return false;
                }
            }
#endif
            return true;
        }

        //GMDG
        private static Dictionary<string, List<Sensor>> _sensorsInstances = new Dictionary<string, List<Sensor>>();    
        //private static List<Sensor> _sensorsInstances = new List<Sensor>();

        internal static void SubscribeSensors(List<Sensor> listOfGeneratedSensors, string configurationName)
        {
            if(!_sensorsInstances.ContainsKey(configurationName))
            {
                _sensorsInstances[configurationName] = new List<Sensor>();
            }
            _sensorsInstances[configurationName].AddRange(listOfGeneratedSensors);
        }

        internal static void UnsubscribeSensors(List<Sensor> listOfGeneratedSensors, string configurationName)
        {
            if(!_sensorsInstances.ContainsKey(configurationName))
                return;
            
            _sensorsInstances[configurationName].RemoveAll(sensor => listOfGeneratedSensors.Contains(sensor));
            

            if (_sensorsInstances[configurationName].Count <= 0) 
            {
                _sensorsInstances.Remove(configurationName);
            } 
        }

        private void Update()
        {
            if (Executor.CanRead(false))
            {
                if (Application.isPlaying)
                {
                    foreach (List<Sensor> sensors in _sensorsInstances.Values)
                    {
                        for (int i = 0; i < sensors.Count; i++)
                        {
                            sensors[i].Update();
                        }
                    }
                    _iteration++;
                }
                Executor.CanRead(true);
            }
        }

        //GMDG

        internal static bool ConfigurationsChanged
        {
            get
            {
                return _configurationsChanged;
            }
            set
            {
                _configurationsChanged = value;
                if (_configurationsChanged)
                {
                    NotifyBrains();
                }
            }
        }

        public static int numberOfSensors
        {
            get
            {
                return _sensorsInstances.Count;
            }
        }
        #region Unity Messages
        void OnDestroy()
        {
            // Debug.Log("destroing");
            destroyed = true;
        }
        void Reset()
        {
            if (FindObjectsOfType<SensorsManager>().Length > 1)
            {
                try
                {
                    throw new Exception("Only one SensorsManager can be instantiated");
                }
                finally
                {
                    if (Application.isPlaying)
                    {
                        Destroy(this);
                    }
                    else
                    {
                        DestroyImmediate(this);
                    }
                }
            }
        }
        /*void Update()
        {
            if (Application.isPlaying)
            {
                if (MOVING_AVG_FRAMES.Count > 30)
                {
                    MOVING_AVG_FRAMES.RemoveAt(0);
                }
                currentFps = 1f / Time.unscaledDeltaTime;
                MOVING_AVG_FRAMES.Add(currentFps);
                avgFps = FPSAvg();
                if ((avgFps <= MIN_AVG_FPS || currentFps <= MIN_CURRENT_FPS) && MAX_MS > 1)
                {
                    MAX_MS--;
                }
                else if (avgFps >= MAX_AVG_FPS && currentFps >= MAX_CURRENT_FPS && MAX_MS < 20)
                {
                    MAX_MS++;
                }
            }
        }*/

        private float FPSAvg()
        {
            if (MOVING_AVG_FRAMES.Count == 0)
            {
                return 0;
            }
            float sum = 0;
            foreach (float d in MOVING_AVG_FRAMES)
            {
                sum += d;
            }
            return sum / MOVING_AVG_FRAMES.Count;
        }

        /*void Start()
        {
            MIN_AVG_FPS = Math.Max(Application.targetFrameRate - 2, 58);
            MIN_CURRENT_FPS = Math.Max(Application.targetFrameRate - 10, 50);
            MAX_AVG_FPS = Math.Max(Application.targetFrameRate, 60);
            MAX_CURRENT_FPS = Math.Max(Application.targetFrameRate - 5, 55);
            MOVING_AVG_FRAMES = new List<float>();
            if (Application.isPlaying)
            {
                destroyed = false;
                Reset();
                StartCoroutine(SensorsUpdate());
            }
        }*/
        /*IEnumerator SensorsUpdate()
        {
            bool first = true;
            while (true)
            {
                updatedSensors = 0;
                Stopwatch localWatch = new Stopwatch();
                watch.Start();
                while (!Executor.CanRead(false))
                {
                    yield return null;
                }
                monoBehaviourManagers = RetrieveSensorsManagers();
                for (int i = 0; i < monoBehaviourManagers.Length; i++)
                {
                    MonoBehaviourSensorsManager manager = monoBehaviourManagers[i];
                    watch.Start();
                    if (manager != null && manager.ready)
                    {
                        manager.ManageSensors();
                        if (watch.ElapsedMilliseconds > MAX_MS)
                        {
                            MS = watch.ElapsedMilliseconds;
                            watch.Reset();
                            yield return null;
                            watch.Start();
                        }
                        if (manager != null) //while yield, the manager could have been destroied
                        {
                            for (int j = 0; j < manager.Sensors.Count; j++)
                            {
                                if (manager != null)
                                {
                                    List<Sensor> sensorsList = manager.Sensors.ElementAt(j).Value;
                                    for (int k = 0; k < sensorsList.Count; k++)
                                    {
                                        Sensor sensor = sensorsList[k];
                                        if (sensor != null)
                                        {
                                            sensor.UpdateValue();
                                            updatedSensors++;
                                        }
                                        if (watch.ElapsedMilliseconds > MAX_MS)
                                        {
                                            MS = watch.ElapsedMilliseconds;
                                            watch.Reset();
                                            yield return null;
                                            watch.Start();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                MS = watch.ElapsedMilliseconds;
                watch.Reset();
                iteration++;
                Executor.CanRead(true);
                yield return null;
                first = false;
            }
        }*/


        void OnApplicationQuit()
        {
            Executor.ShutDownAll();
            destroyed = true;
        }
        internal IEnumerable<string> ConfigurationNames()
        {
            List<string> availableConfigurationNames = new List<string>();
            foreach (SensorConfiguration configuration in Resources.FindObjectsOfTypeAll<SensorConfiguration>())
            {
                availableConfigurationNames.Add(configuration.ConfigurationName);
            }
            return availableConfigurationNames;
        }
        #endregion
        #region Design-time methods
        private static void NotifyBrains()
        {
            foreach (ReactiveBrain brain in Resources.FindObjectsOfTypeAll<ReactiveBrain>())
            {
                brain.sensorsConfigurationsChanged = true;
            }
            ConfigurationsChanged = false;
        }
        internal List<SensorConfiguration> GetConfigurations(List<string> chosenSensorConfigurations)
        {
            List<SensorConfiguration> toReturn = new List<SensorConfiguration>();
            foreach (string confName in chosenSensorConfigurations)
            {
                foreach (SensorConfiguration configuration in Resources.FindObjectsOfTypeAll<SensorConfiguration>())
                {
#if UNITY_EDITOR
                    if (PrefabStageUtility.GetPrefabStage(configuration.gameObject) != null)
                    {
                        continue;
                    }
#endif
                    if (configuration.ConfigurationName == confName)
                    {
                        toReturn.Add(configuration);
                    }
                }
            }
            return toReturn;
        }
        public bool ExistsConfigurationWithName(string name)
        {
            foreach (SensorConfiguration configuration in Resources.FindObjectsOfTypeAll<SensorConfiguration>())
            {
                if (configuration.ConfigurationName ==name )
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #region Run-time methods
        internal bool IsSomeActiveInScene(List<string> configurationNames)
        {
            if (configurationNames.Count == 0)
            {
                return true;
            }
            foreach (string configurationName in configurationNames)
            {
                foreach (SensorConfiguration configuration in FindObjectsOfType<SensorConfiguration>())
                {
                    if (configuration.ConfigurationName == configuration.ConfigurationName )
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        internal static void ReturnSensorsMappings(Brain brain)
        {
            ConcurrentBag<string> mapping = new ConcurrentBag<string>(); ;
            List<Sensor> sensors = RetrieveBrainsSensors(brain);
            Parallel.ForEach(sensors, sensor =>
            {
                if (sensor != null)
                {
                    mapping.Add(brain.ActualSensorEncoding(sensor.Map()));
                }
            });
            brain.sensorsMapping = string.Join("", mapping);

        }
        private static List<Sensor> RetrieveBrainsSensors(Brain brain)
        {
            List<Sensor> sensors = new List<Sensor>();
            foreach (string sensorConf in InstantiatedSensors[brain])
            {
                if (_sensorsInstances.ContainsKey(sensorConf))
                {
                    sensors.AddRange(_sensorsInstances[sensorConf]);
                }
            }
            return sensors;
        }
        public void RegisterBrainsSensorConfigurations(Brain brain, List<string> instantiated)
        {
            if (!InstantiatedSensors.ContainsKey(brain))
            {
                InstantiatedSensors.Add(brain, instantiated);
            }
            else
            {
                InstantiatedSensors[brain] = instantiated;
            }

        }
        #endregion
    }
}
