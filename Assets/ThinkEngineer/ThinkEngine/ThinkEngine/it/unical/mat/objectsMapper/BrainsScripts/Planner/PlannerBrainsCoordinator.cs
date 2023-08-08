using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThinkEngine.Planning
{
    public class PlannerBrainsCoordinator : MonoBehaviour
    {
        private Scheduler scheduler;
        public int priorityExecuting;
        private SortedDictionary<int, Plan> plannersLastPlan;
        internal void SetPriority(int previousPriority, int newPriority, PlannerBrain brain)
        {
            if (!ValidPriority(newPriority))
            {
                throw new Exception("Planner priority should be between 1 and the number of brains associated to the gameobject");
            }
            if (ValidPriority(previousPriority) && previousPriority != newPriority)
            {
                foreach (PlannerBrain currentBrain in GetComponents<PlannerBrain>())
                {
                    if (brain != currentBrain && currentBrain.Priority == newPriority)
                    {
                        currentBrain.Priority = previousPriority;
                    }
                }
            }
        }

        internal void SchedulerIsIdle()
        {
            priorityExecuting = NumberOfBrains() + 1;
            //Debug.Log("set priorityExecuting to " + priorityExecuting);
        }

        private bool ValidPriority(int priority)
        {
            if (priority < 0 || priority >= NumberOfBrains())
            {
                return false;
            }
            return true;
        }

        internal int NumberOfBrains()
        {
            return GetComponents<PlannerBrain>().Count();
        }

        internal void PlanReady(PlannerBrain brain)
        {
            /*if (priorityExecuting >= brain.Priority)
            {
                if (!brain.GetPlan().IsExecutable())
                {
                    return;
                }
                bool used = scheduler.NewPlan(brain.GetPlan());
                if (used)
                {
                    if (plannersLastPlan.ContainsKey(brain.Priority))
                    {
                        plannersLastPlan.Remove(brain.Priority);
                    }
                    priorityExecuting = brain.Priority;
                }
                else
                {
                    plannersLastPlan[brain.Priority] = brain.GetPlan();
                }
            }
            else
            {
                plannersLastPlan[brain.Priority] = brain.GetPlan();
            }*/
            //Debug.Log("Adding plan");
            plannersLastPlan[brain.Priority] = brain.GetPlan();
        }

        void Update()
        {
            CheckIfChangePlan();
        }

        internal void CheckIfChangePlan()
        {
            //Debug.Log("checking if plan ready");
            //priorityExecuting = NumberOfBrains()+1;
            while (plannersLastPlan.Count > 0 && plannersLastPlan.First().Key < priorityExecuting)
            {
                //Debug.Log("plan found");
                KeyValuePair<int, Plan> toExecute = plannersLastPlan.First();
                if (!toExecute.Value.IsExecutable())
                {
                    plannersLastPlan.Remove(toExecute.Key);
                    continue;
                }
                bool used = scheduler.NewPlan(toExecute.Value);
                if (used)
                {
                    //Debug.Log("plan executed");
                    plannersLastPlan.Remove(toExecute.Key);
                    priorityExecuting = toExecute.Key;
                    break;
                }
            }
        }

        void Start()
        {
            scheduler = GetComponent<Scheduler>();
            priorityExecuting = NumberOfBrains() + 1;
            plannersLastPlan = new SortedDictionary<int, Plan>();
        }


        internal void RemoveBrain(int priority)
        {
            foreach (PlannerBrain brain in GetComponents<PlannerBrain>())
            {
                if (brain != null && brain.Priority > priority)
                {
                    brain.Priority--;
                }
            }
        }
    }
}
