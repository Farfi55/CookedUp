#if UNITY_EDITOR
using System;
using ThinkEngine.Mappers;
using UnityEditor;
using UnityEngine;

namespace ThinkEngine.Editors
{
    [CustomEditor(typeof(SensorConfiguration))]
    class SensorsConfigurationEditor : AbstractConfigurationEditor
    {
        SensorConfiguration sensorConfiguration;
        private bool configurePropertyMode;
        private MyListString actualProperty;
        string tempPropertyName;
        void Reset()
        {
            sensorConfiguration = target as SensorConfiguration;
            base.Reset();
        }
        override public void OnInspectorGUI()
        {
            if (!configurePropertyMode)
            {
                DrawPropertiesExcluding(serializedObject, new string[] { });
                serializedObject.ApplyModifiedProperties();
                base.OnInspectorGUI();
                 bool generate = GUILayout.Button("Generate Sensors Code");
                if (generate && configuration.ToMapProperties.Count > 0)
                {
                    CodeGenerator.GenerateCode(configuration.ToMapProperties, configuration.gameObject, sensorConfiguration);
                }
            }
            else
            {
                ConfigureProperty();
            }
        }

        private void ConfigureProperty()
        {
            EditorGUILayout.HelpBox("Configure advanced feature of the property",MessageType.Info);
            PropertyFeatures features = configuration.PropertyFeaturesList.Find(x => x.property.Equals(actualProperty));
            EditorGUILayout.BeginHorizontal();
            tempPropertyName = EditorGUILayout.TextField("Property Alias:", tempPropertyName);
            if (GUILayout.Button("Save"))
            {
                try
                {
                    features.PropertyAlias = tempPropertyName;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "InvalidName")
                    {
                        Debug.LogError("The name " + tempPropertyName + " can not be used.");
                        tempPropertyName = features.PropertyAlias;
                        GUI.FocusControl(null);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            features.windowWidth = EditorGUILayout.IntSlider("Aggregation Window:", features.windowWidth, 1, 200);
            ShowOperationChoice(features);
            serializedObject.ApplyModifiedProperties();
            if (GUILayout.Button("Done", GUILayout.Width(200)))
            {
                configurePropertyMode = false;
                actualProperty = null;
                tempPropertyName = configuration.ConfigurationName;
            }
        }

        private void ShowOperationChoice(PropertyFeatures features)
        {
            if (MapperManager.NeedsAggregates(configuration.ObjectTracker.PropertyType(actualProperty)))
            {
                //int propertyIndex = actualProperty.GetHashCode();
                int oldOperation = features.operation;
                Type propertyType = sensorConfiguration.ObjectTracker.PropertyType(actualProperty);
                string[] displayedOptions = Enum.GetNames(MapperManager.GetAggregationTypes(propertyType));
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                int newOperation = EditorGUILayout.Popup(oldOperation, displayedOptions);
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();

                if (newOperation != oldOperation)
                {
                    sensorConfiguration.SetOperationPerProperty(actualProperty, newOperation);
                }
                if (MapperManager.GetAggregationStreamOperationsIndexes(propertyType).Contains(newOperation) )
                {
                    string oldValue = features.specificValue;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    string newValue = EditorGUILayout.TextField("Value to track", oldValue);
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    if (!oldValue.Equals(newValue))
                    {
                        try
                        {
                            Convert.ChangeType(newValue, propertyType);
                            features.specificValue = newValue;
                        }
                        catch
                        {
                            Debug.LogError(newValue + " value not valid for property of type " + propertyType);
                        }
                    }
                    int oldCounter = features.counter;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    int newCounter = EditorGUILayout.IntSlider("Steps", oldCounter,1, features.windowWidth);
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    if (!oldCounter.Equals(newCounter))
                    {
                        features.counter = newCounter;
                    }
                    
                }
            }
        }

        protected override void SpecificFields(MyListString property)
        {
            PropertyFeatures propertyFeatures = configuration.PropertyFeaturesList.Find(x => x.property.Equals(property));
            if (propertyFeatures == null)
            {
                return;
            }
            string alias = propertyFeatures.PropertyAlias;
            EditorGUILayout.LabelField("Alias: "+ alias);
            if (GUILayout.Button("Configure"))
            {
                configurePropertyMode = true;
                actualProperty = property;
                tempPropertyName = alias;
            }

        }

        /*
protected override void SpecificFields(MyListString property)
{
   sensorConfiguration = configuration as SensorConfiguration;
   if (MapperManager.NeedsAggregates(configuration.ObjectTracker.PropertyType(property)))
   {
       int propertyIndex = property.GetHashCode();
       int oldOperation = sensorConfiguration.OperationPerProperty[propertyIndex];
       Type propertyType = sensorConfiguration.ObjectTracker.PropertyType(property);
       string[] displayedOptions = Enum.GetNames(MapperManager.GetAggregationTypes(propertyType));
       int newOperation = EditorGUILayout.Popup(oldOperation, displayedOptions);
       if (newOperation != oldOperation)
       {
           sensorConfiguration.SetOperationPerProperty(property, newOperation);
           if (newOperation == MapperManager.GetAggregationSpecificIndex(propertyType))
           {
               string oldValue = sensorConfiguration.SpecificValuePerProperty[propertyIndex];
               string newValue = EditorGUILayout.TextField("Value to track", oldValue);
               if (!oldValue.Equals(newValue))
               {
                   sensorConfiguration.SetSpecificValuePerProperty(property, newValue);
               }
           }
       }
   }
}
*/
    }
}
#endif