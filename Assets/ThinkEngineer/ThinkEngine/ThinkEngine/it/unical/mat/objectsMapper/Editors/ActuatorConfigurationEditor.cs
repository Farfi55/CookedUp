#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThinkEngine.Editors
{
    [CustomEditor(typeof(ActuatorConfiguration))]
    class ActuatorConfigurationEditor : AbstractConfigurationEditor
    {
        protected override void SpecificFields(MyListString property)
        {

        }

        void OnDestroy()
        {
            if (!Application.isPlaying && target == null && go != null)
            {
                if (go.GetComponent<ActuatorConfiguration>() == null)
                {
                    DestroyImmediate(go.GetComponent<MonoBehaviourActuatorsManager>());
                }
            }
        }
        int chosenMethod;
        List<string> methodsToShow;
        new void Reset()
        {
            base.Reset();
            methodsToShow = Utility.TriggerMethodsToShow;
            methodsToShow.Add("Always");
            chosenMethod = Utility.GetTriggerMethodIndex(((ActuatorConfiguration)target).MethodToApply);
        }
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                DrawDefaultInspector();
                return;
            }
            GUI.enabled = false;
            EditorGUILayout.TextField("Trigger Script Path", Utility.TriggerClassPath);
            GUI.enabled = true;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Choose when to apply the reasoner actions");
            chosenMethod = EditorGUILayout.Popup(chosenMethod, methodsToShow.ToArray());
            EditorGUILayout.EndHorizontal();
            ((ActuatorConfiguration)target).MethodToApply = methodsToShow[chosenMethod];
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}
#endif