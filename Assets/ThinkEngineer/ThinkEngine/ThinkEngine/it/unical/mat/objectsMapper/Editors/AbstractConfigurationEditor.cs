#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using ThinkEngine.Mappers;
using UnityEditor;
using UnityEngine;

namespace ThinkEngine.Editors
{
    internal delegate bool ToggleType(string propertyName, bool value);
    [CustomEditor(typeof(AbstractConfiguration))]
    internal abstract class AbstractConfigurationEditor : Editor
    {
        string temporaryName;
        protected AbstractConfiguration configuration;
        static readonly Dictionary<bool, ToggleType> toggleType;
        protected GameObject go;
        static GUIStyle redText;
        static GUIStyle toUse;

        protected void Reset()
        {
            configuration = target as AbstractConfiguration;
            go = configuration.gameObject;
            temporaryName = configuration.ConfigurationName;
        }
       
        static AbstractConfigurationEditor()
        {
            toggleType = new Dictionary<bool, ToggleType>
            {
                [true] = ToggleAsFoldout,
                [false] = ToggleAsToggleLeft
            };
        }

        private static bool ToggleAsToggleLeft(string property, bool value)
        {
            return EditorGUILayout.ToggleLeft(property, value);
        }

        private static bool ToggleAsFoldout(string property, bool value)
        {
            EditorGUI.indentLevel++; //foldout is misplaced wrt toggle
            bool toReturn = EditorGUILayout.Foldout(value, property);
            EditorGUI.indentLevel--;
            return toReturn;
        }
       
        override public void OnInspectorGUI()
        {
            if (redText == null)
            {
                redText = new GUIStyle(EditorStyles.textField);
                redText.normal.textColor = Color.red;
                redText.focused.textColor = Color.red;
                toUse = EditorStyles.textField;
            }
            if (Application.isPlaying)
            {
                DrawDefaultInspector();
                return;
            }
            try
            {
               
                ConfigurationName();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                bool refresh = GUILayout.Button("Refresh property hierarchy.");
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Choose the properties to map.", MessageType.Info);
                if (refresh)
                {
                    configuration.RefreshObjectTracker();
                }
                ListProperties(new MyListString(), true);
                bool clear = GUILayout.Button("Clear");
                if (clear)
                {
                    configuration.Clear();
                    temporaryName = configuration.ConfigurationName;
                }
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Reset();
            }
        }
        private void ConfigurationName()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            temporaryName = EditorGUILayout.TextField("Configuration name", temporaryName, toUse);
            if (!configuration.IsAValidName(temporaryName))
            {
                GUI.enabled = false;
                toUse = redText;
            }
            else
            {
                toUse = EditorStyles.textField;
            }
            bool save = GUILayout.Button("Save");
            if (GUILayout.Button("RefreshDefaultAlias"))
            {
                configuration.RefreshDefaultPropertiesAlias();
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            try
            {
                if (save)
                {
                    configuration.ConfigurationName = temporaryName;
                }
            }
            catch
            {
                if (temporaryName.Equals(""))
                {
                    if (EditorUtility.DisplayDialog("Error", "Configuration name can not be empty.", "Ok."))
                    {
                        temporaryName = configuration.ConfigurationName;
                        GUI.FocusControl(null);
                    }
                }else if (EditorUtility.DisplayDialog("Error", "Another configuration named " + temporaryName + " exists. Please, choose a different name.", "Ok."))
                {
                    temporaryName = configuration.ConfigurationName;
                    GUI.FocusControl(null);
                }
            }
        }


        private void ListProperties(MyListString startingProperty, bool needsSpecifications)
        {
            float originalLabelSize = EditorGUIUtility.labelWidth;

            EditorGUIUtility.labelWidth = 7;
            List<MyListString> firstLevel = GetProperties(startingProperty);
            foreach (MyListString property in firstLevel)
            {
                bool wasActive = IsActive(property);
                EditorGUILayout.BeginHorizontal();

                bool isActive = toggleType[IsExpandable(property)](property[property.Count-1], wasActive);
                if (wasActive != isActive)
                {
                    configuration.ToggleProperty(property, isActive);
                }
                if (isActive && needsSpecifications)
                {
                    
                    SpecificFields(property);
                }
                EditorGUILayout.EndHorizontal();
                if (isActive && IsExpandable(property))
                {
                    EditorGUI.indentLevel++;
                    bool willNeedSpecifications = MapperManager.NeedsSpecifications(configuration.ObjectTracker.PropertyType(property));
                    bool hasToAddSpecification = needsSpecifications && willNeedSpecifications;
                    ListProperties(property, hasToAddSpecification);
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUIUtility.labelWidth = originalLabelSize;
        }
        
        List<MyListString> GetProperties(MyListString startingPoint = null)
        {
            return configuration.ObjectTracker.GetMemberProperties(startingPoint);
        }
        bool IsActive(MyListString property)
        {
            return configuration.IsPropertySelected(property);
        }
        bool IsExpandable(MyListString property)
        {
            return configuration.ObjectTracker.IsPropertyExpandable(property);
        }
        protected abstract void SpecificFields(MyListString property);


    }
}
#endif