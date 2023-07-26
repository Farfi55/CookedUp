#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThinkEngine.Editors
{
    [CustomEditor(typeof(ReactiveBrain))]
    public class ReactiveBrainEditor : BrainEditor
    {
        List<string> _actuatorsConfigurationNames;
        Dictionary<string, bool> _toggledActuatorsConfigurations;
        private ReactiveBrain myScript;
        protected override Brain Target
        {
            get
            {
                return myScript;
            }
        }

        List<string> ActuatorsConfigurationNames
        {
            get
            {
                if (_actuatorsConfigurationNames == null)
                {
                    _actuatorsConfigurationNames = new List<string>();
                }
                return _actuatorsConfigurationNames;
            }
        }

        Dictionary<string, bool> ToggledActuatorsConfigurations
        {
            get
            {
                if (_toggledActuatorsConfigurations == null)
                {
                    _toggledActuatorsConfigurations = new Dictionary<string, bool>();
                }
                return _toggledActuatorsConfigurations;
            }
        }
        protected override void ReadingFromBrain()
        {
            base.ReadingFromBrain();
            bool delete = false;
            foreach (string actuatorConfName in myScript.ChosenActuatorConfigurations)
            {
                if (!Utility.ActuatorsManager.ExistsConfigurationWithName(actuatorConfName, myScript))
                {
                    delete = true;
                    continue;
                }
                ToggledActuatorsConfigurations[actuatorConfName] = true;
            }
            if (delete)
            {
                myScript.RemoveNullActuatorConfigurations();
            }
        }
        protected override void BasicConfiguration()
        {
            base.BasicConfiguration();
            RemoveUnexistingActuators();
            AddNewActuatorsConfigurations();
        }
        protected override void OnEnable()
        {
            myScript = target as ReactiveBrain;
            base.OnEnable();
        }



        private void RemoveUnexistingActuators()
        {
            List<string> toDelete = new List<string>();
            bool removed = false;
            foreach (string actuatorName in ActuatorsConfigurationNames)
            {
                if (!Utility.ActuatorsManager.ExistsConfigurationWithName(actuatorName, myScript))
                {
                    toDelete.Add(actuatorName);
                    ToggledActuatorsConfigurations.Remove(actuatorName);
                    removed = true;
                }
            }
            foreach (string current in toDelete)
            {
                ActuatorsConfigurationNames.Remove(current);
            }
            if (removed)
            {
                myScript.RemoveNullActuatorConfigurations();
            }
        }

        private void AddNewActuatorsConfigurations()
        {
            foreach (string actuatorConfName in Utility.ActuatorsManager.AvailableConfigurationNames(myScript))
            {
                AddConfigurationName(actuatorConfName, ActuatorsConfigurationNames, ToggledActuatorsConfigurations);
            }
        }


        protected override void ListAvailableConfigurations()
        {
            base.ListAvailableConfigurations();
            if (!myScript.prefabBrain)
            {
                EditorGUILayout.LabelField("All the available Actuator Configurations", EditorStyles.boldLabel);
            }
            else
            {
                EditorGUILayout.LabelField(myScript.gameObject.name + " available Actuator Configurations", EditorStyles.boldLabel);
            }
            ShowConfigurations(ActuatorsConfigurationNames, ToggledActuatorsConfigurations, true);
        }

        protected override void IfConfigurationChanged()
        {
            base.IfConfigurationChanged();

            if (myScript.actuatorsConfigurationsChanged)
            {
                RemoveUnexistingActuators();
                AddNewActuatorsConfigurations();
                myScript.actuatorsConfigurationsChanged = false;
            }
        }

        protected override void CheckIfDisableGUI(string confName)
        {
            base.CheckIfDisableGUI(confName);
            ReactiveBrain assignedTo = Utility.ActuatorsManager.AssignedTo(confName);
            if (assignedTo != null && !myScript.ChosenActuatorConfigurations.Contains(confName))
            {
                GUI.enabled = false;
            }
        }
        protected override void SavingInBrain()
        {
            base.SavingInBrain();
            SaveConfigurations(ToggledActuatorsConfigurations, myScript.ChosenActuatorConfigurations);
        }
    }
}

#endif