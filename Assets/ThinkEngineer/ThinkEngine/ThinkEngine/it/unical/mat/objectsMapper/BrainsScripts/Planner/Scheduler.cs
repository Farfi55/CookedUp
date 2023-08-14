using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThinkEngine.Planning
{

    public class Scheduler : MonoBehaviour
    {
        internal Plan currentPlan;
        private bool _isIdle;
        Coroutine planExecutionCoroutine;
        public int ResidualActions
        {
            get
            {
                if (currentPlan == null)
                {
                    return 0;
                }
                return currentPlan.actions.Count;
            }
        }
        public bool IsIdle
        {
            get
            {
                return _isIdle;
            }
            internal set
            {
                _isIdle = value;
                if (_isIdle)
                {
                    //Debug.Log("requesting new plan");
                    GetComponent<PlannerBrainsCoordinator>().SchedulerIsIdle();
                }
            }
        }


        internal bool NewPlan(Plan plan)
        {
            if (currentPlan != null && currentPlan.IsExecuting)
            {
                if (planExecutionCoroutine != null)
                {
                    StopCoroutine(planExecutionCoroutine);
                }
            }
            currentPlan = plan;
            planExecutionCoroutine = StartCoroutine(currentPlan.ApplyPlan(this));
            return true;
        }
    }
}
