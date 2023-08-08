using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ThinkEngine.Planning
{
    public class Plan
    {
        internal static int counter = 0;
        public int myCount;
        internal List<Action> actions;
        public int priority;
        private bool _isExecuting;
        public bool IsExecuting
        {
            get
            {
                return _isExecuting;
            }
        }
        internal Plan()
        {
            myCount = ++counter;
            actions = new List<Action>();
        }

        public Plan(List<Action> lists)
        {
            actions = new List<Action>();
            for (int i = 0; i < lists.Count; i++)
            {
                actions.Add(lists[i]);
                actions[i].belongingTO = this;
            }
        }

        private Action Next()
        {
            if (actions.Count == 0)
            {
                Debug.LogError("Empty plan");
            }
            Action toReturn = actions[0];
            actions.RemoveAt(0);
            return toReturn;
        }
        private State PlanState(bool finalize=false)
        {
            List<Action> toRemove = new List<Action>();
            while (actions.Count > 0)
            {
                actions[0].Init();
                State state = actions[0].Prerequisite();
                if (state == State.SKIP)
                {
                    actions[0].Clean();
                    actions.RemoveAt(0);
                }
                else
                {
                    if (finalize)
                    {
                        actions[0].Clean();
                    }
                    return state;
                }
            }
            return State.ABORT;
        }

        internal bool IsExecutable()
        {
            return !PlanState(true).Equals(State.ABORT);
        }

        internal IEnumerator ApplyPlan(Scheduler scheduler)
        {
            if (actions.Count == 0)
            {
                try
                {
                    yield break;
                }
                finally
                {
                    scheduler.IsIdle = true;
                }
            }
            scheduler.IsIdle = false;
            _isExecuting = true;
            while (actions.Count > 0)
            {

                State planState = State.WAIT;
                while (PlanWaiting(ref planState))
                {
                    yield return null;
                }
                if (planState == State.READY)
                {
                    Action next = Next();
                    next.Do();
                    yield return null;
                    State done = next.Done();
                    while (done == State.WAIT)
                    {
                        yield return null;
                        done = next.Done();
                    }
                    next.Clean();
                    if (done == State.ABORT)
                    {
                        break;
                    }
                }
                else
                {
                    //Debug.Log("Breaking for abort");
                    if (actions.Count > 0) //actions could be empty if State is ABORT due to all actions being skipped
                    {
                        actions[0].Clean();
                    }
                    break;
                }
            }
            _isExecuting = false;
            scheduler.IsIdle = true;
            //Debug.Log("Scheduler is idle");
            scheduler.currentPlan = null;
        }

        bool PlanWaiting(ref State planState)
        {
            planState = PlanState();
            return planState == State.WAIT;
        }
    }

}
