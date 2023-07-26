using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Collections;
using ThinkEngine.it.unical.mat.objectsMapper.BrainsScripts;
using ThinkEngine.it.unical.mat.objectsMapper.BrainsScripts.Actuator;
using BrainsScripts.Specialitations.ASP;

namespace ThinkEngine
{
    [ExecuteAlways, RequireComponent(typeof(IndexTracker))]
    public class ReactiveBrain : Brain
    {
        #region Serialized Fieds

        [SerializeField, HideInInspector]
        private List<string> _chosenActuatorConfigurations;

        #endregion
        internal List<string> ChosenActuatorConfigurations
        {
            get
            {
                if (_chosenActuatorConfigurations == null)
                {
                    _chosenActuatorConfigurations = new List<string>();
                }
                return _chosenActuatorConfigurations;
            }
        }

        protected override HashSet<string> SupportedFileExtensions
        {
            get
            {
                return new HashSet<string> { "asp" };
            }
        }

        IActualReactiveBrain actuatorBrain;
        #region Runtime Fields
        internal string objectsIndexes;
        internal bool actuatorsConfigurationsChanged;
        #endregion

        #region Unity Messages



        #endregion
        protected override string SpecificFileParts()
        {
            string toReturn = "%Actuators:\n";
            HashSet<string> seenActuatorConfNames = new HashSet<string>();
            foreach (ActuatorConfiguration actuatorConf in Utility.ActuatorsManager.GetCorrespondingConfigurations(ChosenActuatorConfigurations))
            {
                if (seenActuatorConfNames.Contains(actuatorConf.ConfigurationName))
                {
                    continue;
                }
                seenActuatorConfNames.Add(actuatorConf.ConfigurationName);
                foreach (MyListString property in actuatorConf.ToMapProperties)
                {
                    string actuatorMappingAsASP = MapperManager.GetASPTemplate(actuatorConf.ConfigurationName, actuatorConf.gameObject, property);
                    toReturn += ActualActuatorEncoding(actuatorMappingAsASP);
                }
            }
            return toReturn;
        }

        internal string ActualActuatorEncoding(string actuatorMappingAsASP)
        {
            if (actuatorBrain != null)
            {
                return actuatorBrain.ActualActuatorEncoding(actuatorMappingAsASP);
            }
            return actuatorMappingAsASP;
        }
        protected override void Start()
        {
            if (FileExtension.Equals("asp"))
            {
                actuatorBrain = new ASPReactiveBrain();
            }
            base.Start();
        }
        protected override IEnumerator Init()
        {
            if (actuatorBrain != null)
            {
                yield return StartCoroutine(base.Init());
                executor = new ASPReactiveExecutor(this);
                PrepareActuators();
                string GOname = gameObject.name;
                executorName = "executor " + GOname;
                executionThread = new Thread(() =>
                {
                    Thread.CurrentThread.Name = executorName;
                    Thread.CurrentThread.IsBackground = true;
                    executor.Run();
                });
                executionThread.Start();
            }
        }
        internal void RemoveNullActuatorConfigurations()
        {
            ChosenActuatorConfigurations.RemoveAll(x => !Utility.ActuatorsManager.ExistsConfigurationWithName(x, this));
        }
        private void PrepareActuators()
        {
            Utility.ActuatorsManager.RegisterBrainActuatorConfigurations(this, ChosenActuatorConfigurations);
        }

        protected override bool SomeConfigurationAvailable()
        {
            return base.SomeConfigurationAvailable() && Utility.ActuatorsManager.IsSomeActiveInScene(ChosenActuatorConfigurations);
        }

        internal override string ActualSensorEncoding(string sensorsAsASP)
        {
            if (actuatorBrain != null)
            {
                return actuatorBrain.ActualSensorEncoding(sensorsAsASP);
            }
            return sensorsAsASP;
        }
    }
}