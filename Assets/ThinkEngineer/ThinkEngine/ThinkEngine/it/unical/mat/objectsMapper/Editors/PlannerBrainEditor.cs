
#if UNITY_EDITOR
using System.Collections.Generic;
using ThinkEngine.Planning;
using UnityEditor;
using UnityEngine;

namespace ThinkEngine.Editors
{
    [CustomEditor(typeof(PlannerBrain))]
    class PlannerBrainEditor : BrainEditor
    {
        private GameObject go;
        private int priority;
        private List<int> _priorities;
        private List<int> Priorities
        {
            get
            {
                if (_priorities == null)
                {
                    _priorities = new List<int>();
                }
                if(_priorities.Count!= Target.GetComponent<PlannerBrainsCoordinator>().NumberOfBrains())
                {
                    int numberOfBrains = Target.GetComponent<PlannerBrainsCoordinator>().NumberOfBrains();
                    _priorities = new List<int>();
                    for (int i = 0; i < numberOfBrains; i++)
                    {
                        _priorities.Add(i);
                    }
                }
                return _priorities;
            }
        }
        private List<string> _priorityNames;
        private List<string> PriorityNames
        {
            get
            {
                if (_priorityNames == null)
                {
                    _priorityNames = new List<string>();
                }
                if (_priorityNames.Count != Priorities.Count)
                {
                    _priorityNames = new List<string>();
                    for(int i=0; i < Priorities.Count; i++)
                    {
                        _priorityNames.Add("" + (i + 1));
                    }
                }
                return _priorityNames;
            }
        }
        protected override Brain Target
        {
            get
            {
                return (PlannerBrain)target;
            }
        }
        protected override string GetAIFilesPrefixSpecifications()
        {
            return base.GetAIFilesPrefixSpecifications()+"Planner"+(((PlannerBrain)target).Priority+1);
        }
        protected override void ShowSpecificFields()
        {
            base.ShowSpecificFields();
            ((PlannerBrain)target).Priority = EditorGUILayout.IntPopup("Brain Priority: ",((PlannerBrain)target).Priority,PriorityNames.ToArray(),Priorities.ToArray());
            priority = ((PlannerBrain)target).Priority;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            go = Target.gameObject;
        }
        void OnDestroy()
        {
            if (target == null && go != null)
            {
                go.GetComponent<PlannerBrainsCoordinator>().RemoveBrain(priority);
            }
        }
    }
}
#endif
