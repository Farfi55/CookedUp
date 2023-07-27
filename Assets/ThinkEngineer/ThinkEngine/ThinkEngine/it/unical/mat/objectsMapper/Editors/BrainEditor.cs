#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static ThinkEngine.Brain;

namespace ThinkEngine.Editors
{
    [CustomEditor(typeof(Brain))]
    public class BrainEditor : Editor
    {
        protected List<string> _excludedProperties;
        protected List<string> _methodsToShow;
        protected List<string> _sensorsConfigurationNames;
        protected Dictionary<string, bool> _toggledSensorsConfigurations;
        protected Dictionary<string, bool> _availableSolvers;
        Brain brainTarget;
        protected virtual Brain Target
        {
            get
            {
                return brainTarget;
            }
        }

        protected int reasoningExecutionIndex = 0;

        protected List<string> ExcludedProperties
        {
            get
            {
                if (_excludedProperties == null)
                {
                    _excludedProperties = new List<string>();
                }
                if (!_excludedProperties.Contains("executeReasonerOn"))
                {
                    _excludedProperties.Add("executeReasonerOn");
                }
                return _excludedProperties;
            }
        }
        protected List<string> MethodsToShow
        {
            get
            {
                if (_methodsToShow == null)
                {
                    _methodsToShow = new List<string>();
                }
                return _methodsToShow;
            }
            set
            {
                _methodsToShow = value;
            }
        }
        protected List<string> SensorsConfigurationNames
        {
            get
            {
                if (_sensorsConfigurationNames == null)
                {
                    _sensorsConfigurationNames = new List<string>();
                }
                return _sensorsConfigurationNames;
            }
        }
        protected Dictionary<string, bool> ToggledSensorsConfigurations
        {
            get
            {
                if (_toggledSensorsConfigurations == null)
                {
                    _toggledSensorsConfigurations = new Dictionary<string, bool>();
                }
                return _toggledSensorsConfigurations;
            }
        }
        protected Dictionary<string, bool> AvailableSolvers
        {
            get
            {
                if (_availableSolvers == null)
                {
                    _availableSolvers = new Dictionary<string, bool>();
                }
                return _availableSolvers;
            }
        }
        protected virtual void OnEnable()
        {
            brainTarget = (Brain)target;
            Utility.LoadPrefabs();
            brainTarget.prefabBrain = PrefabStageUtility.GetCurrentPrefabStage() != null;
            MyReset();
        }
        void MyReset()
        {
            BasicConfiguration();
            ReadingFromBrain();
        }
        protected virtual void ReadingFromBrain()
        {
            reasoningExecutionIndex = Utility.GetTriggerMethodIndex(brainTarget.ExecuteReasonerOn);
            bool delete = false;
            foreach (string sensorConfName in brainTarget.ChosenSensorConfigurations)
            {
                if (!Utility.SensorsManager.ExistsConfigurationWithName(sensorConfName))
                {
                    delete = true;
                    continue;
                }
                ToggledSensorsConfigurations[sensorConfName] = true;
            }
            if (delete)
            {
                brainTarget.RemoveNullSensorConfigurations();
            }
        }
        private void RemoveUnexistingSensors()
        {
            List<string> toDelete = new List<string>();
            bool removed = false;
            foreach (string sensorName in SensorsConfigurationNames)
            {
                if (!Utility.SensorsManager.ExistsConfigurationWithName(sensorName))
                {
                    toDelete.Add(sensorName);
                    ToggledSensorsConfigurations.Remove(sensorName);
                    removed = true;
                }
            }
            if (removed)
            {
                (brainTarget).RemoveNullSensorConfigurations();
            }
            foreach (string current in toDelete)
            {
                SensorsConfigurationNames.Remove(current);
            }
        }
        private void AddNewSensorsConfigurations()
        {
            foreach (string sensorConfName in Utility.SensorsManager.ConfigurationNames())
            {
                AddConfigurationName(sensorConfName, SensorsConfigurationNames, ToggledSensorsConfigurations);
            }
        }

        protected void AddConfigurationName(string configuration, List<string> configurationsNames, Dictionary<string, bool> toggledConfigurations)
        {
            if (!configurationsNames.Contains(configuration))
            {
                configurationsNames.Add(configuration);
            }
            if (!toggledConfigurations.ContainsKey(configuration))
            {
                toggledConfigurations.Add(configuration, false);
            }
        }
        protected virtual void BasicConfiguration()
        {
            MethodsToShow = Utility.TriggerMethodsToShow;
            MethodsToShow.Add("When Sensors are ready");
            AddNewSensorsConfigurations();
            RemoveUnexistingSensors();
        }
        protected void GenerateAITemplateFileButton()
        {
            string paradigm = brainTarget.FileExtension.Equals("") ? "ASP-like" : brainTarget.FileExtension.ToUpper();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate "+paradigm+" file template", GUILayout.Width(200)))
            {
                brainTarget.GenerateFile();
            }
            if (GUILayout.Button("Show in explorer"))
            {
                EditorUtility.OpenWithDefaultApp(Utility.TemplatesFolder);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Generating a new file will delete the previous template!", MessageType.Warning);
        }
        protected void ChooseReasonerTriggerMethod()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Choose when to run the reasoner");
            reasoningExecutionIndex = EditorGUILayout.Popup(reasoningExecutionIndex, MethodsToShow.ToArray());
            EditorGUILayout.EndHorizontal();
            brainTarget.ExecuteReasonerOn = MethodsToShow[reasoningExecutionIndex];
        }
        protected virtual void ListAvailableConfigurations()
        {
            EditorGUILayout.LabelField("In scene Sensor Configurations", EditorStyles.boldLabel);
            ShowConfigurations(SensorsConfigurationNames, ToggledSensorsConfigurations);
        }
        protected void ShowConfigurations(List<string> configurationNames, Dictionary<string, bool> toggledConfigurations, bool checkIfDisableGUI = false)
        {
            foreach (string confName in configurationNames)
            {
                if (checkIfDisableGUI)
                {
                    CheckIfDisableGUI(confName);
                }
                if (toggledConfigurations.ContainsKey(confName))
                {
                    toggledConfigurations[confName] = EditorGUILayout.ToggleLeft(confName, toggledConfigurations[confName]);
                }
                GUI.enabled = true;
            }
        }

        protected virtual void SavingInBrain()
        {
            SaveConfigurations(ToggledSensorsConfigurations, brainTarget.ChosenSensorConfigurations);
        }

        protected void SaveConfigurations(Dictionary<string, bool> toggledConfigurations, List<string> chosenConfigurations)
        {
            foreach (string configurationName in toggledConfigurations.Keys)
            {
                if (toggledConfigurations[configurationName])
                {
                    if (!chosenConfigurations.Contains(configurationName))
                    {
                        chosenConfigurations.Add(configurationName);
                    }
                }
                else if (chosenConfigurations.Contains(configurationName))
                {
                    chosenConfigurations.Remove(configurationName);
                }
            }
        }
        private void ExclusiveTogglesCheck(string currentTrue)
        {
            if (brainTarget.specificAIFile && brainTarget.globalAIFile)
            {
                if (currentTrue.Equals("Specific"))
                {
                    brainTarget.specificAIFile = false;
                }
                else
                {
                    brainTarget.globalAIFile = false;
                }
            }
            if (!brainTarget.specificAIFile && !brainTarget.globalAIFile)
            {
                brainTarget.globalAIFile = true;
            }
        }
        private void ShowNotEditableInformations()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Trigger Script"))
            {
                System.Diagnostics.Process.Start(Path.Combine(".", Utility.TriggerClassPath));
            }
            if (GUILayout.Button("Open AI Files folder"))
            {
                EditorUtility.OpenWithDefaultApp(Path.Combine(brainTarget.AIFilesPath));
            }
            EditorGUILayout.EndHorizontal();
        }

        protected virtual string GetAIFilesPrefixSpecifications() 
        {
            return "";
        }

        public override void OnInspectorGUI()
        {
            if (!CheckSolver())
            {
                EditorGUILayout.HelpBox("Please, download a compatible solver suitable for your system. See documentation at \"How to add ThinkEngine to an existing project\".", MessageType.Error, true);
                return;
            }
            IfConfigurationChanged();
            DrawPropertiesExcluding(serializedObject, ExcludedProperties.ToArray());
            EditorGUILayout.BeginHorizontal();
            Target.maintainInputFile = EditorGUILayout.Toggle("Maintain input file", Target.maintainInputFile);
            if (GUILayout.Button("Open input folder"))
            {
                EditorUtility.OpenWithDefaultApp(Path.Combine(Path.GetTempPath(),"ThinkEngineFacts"));
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUIContent label = new GUIContent("Choose the solver");
            brainTarget.solverEnum = (SOLVER)EditorGUILayout.EnumPopup(label, brainTarget.solverEnum, CheckIfAvailable,true);
            EditorGUILayout.EndHorizontal();
            ShowSpecificFields();
            ShowNotEditableInformations();
            ListAvailableConfigurations();
            ChooseReasonerTriggerMethod();
            serializedObject.ApplyModifiedProperties();
            GenerateAITemplateFileButton();
            SavingInBrain();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(Target);
            }
        }

        bool CheckIfAvailable(Enum solver)
        {
            return SolversChecker.AvailableSolvers().Contains((SOLVER)solver);
        }

        private bool CheckSolver()
        {
            List<SOLVER> available = SolversChecker.AvailableSolvers();
            AvailableSolvers["dlv"] = available.Contains(SOLVER.DLV2);
            AvailableSolvers["clingo"] = available.Contains(SOLVER.Clingo);
            AvailableSolvers["clingo"] = available.Contains(SOLVER.Incremental_DLV2);
            return AvailableSolvers.ContainsValue(true);
        }

        protected virtual void IfConfigurationChanged()
        {
            if (brainTarget.sensorsConfigurationsChanged)
            {
                RemoveUnexistingSensors();
                AddNewSensorsConfigurations();
                brainTarget.sensorsConfigurationsChanged = false;
            }

        }

        protected virtual void CheckIfDisableGUI(string confName){}
        protected virtual void ShowSpecificFields() { }
    }
}
#endif