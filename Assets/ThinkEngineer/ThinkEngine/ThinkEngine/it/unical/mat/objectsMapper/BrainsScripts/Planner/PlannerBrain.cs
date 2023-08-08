using it.unical.mat.embasp.languages.asp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace ThinkEngine.Planning
{
    [RequireComponent(typeof(Scheduler)), RequireComponent(typeof(PlannerBrainsCoordinator))]
    class PlannerBrain : Brain, IComparable<PlannerBrain>
    {

        private PlannerBrainsCoordinator _coordinator;
        internal PlannerBrainsCoordinator Coordinator
        {
            get
            {
                if (_coordinator == null)
                {
                    _coordinator = GetComponent<PlannerBrainsCoordinator>();
                }
                return _coordinator;
            }
        }

        [SerializeField, HideInInspector]
        private int _priority = -1;
        Plan plan;
        [HideInInspector]
        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                int previous = _priority;
                try
                {
                    _priority = value;
                    Coordinator.SetPriority(previous, _priority, this);
                }
                catch (Exception e)
                {
                    _priority = previous;
                    Debug.LogError(e);
                }
            }
        }
        IActualPlannerBrain _planner;
        IActualPlannerBrain Planner
        {
            get
            {
                if (_planner == null)
                {
                    if (FileExtension.Equals("asp"))
                    {
                        _planner = new ASPPlannerBrain();
                    }
                }
                return _planner;
            }
        }
        protected override HashSet<string> SupportedFileExtensions
        {
            get
            {
                return new HashSet<string> { "asp" };
            }
        }

        internal void PlanAvailable(object plan)
        {
            Planner.NewPlanAvailable(plan);
        }

        internal Plan GetPlan()
        {
            return plan ?? new Plan();
        }

        void Reset()
        {
            if (Priority == -1)
            {
                Priority = GetComponents<PlannerBrain>().Count() - 1;
            }
        }
        protected override IEnumerator Init()
        {
            if (Planner != null)
            {
                yield return StartCoroutine(base.Init());
                executor = Planner.GetPlannerExecutor(this);
                executorName = "Solver executor " + gameObject.name + Priority;

                executionThread = new Thread(() =>
                {
                    Thread.CurrentThread.Name = executorName;
                    Thread.CurrentThread.IsBackground = true;
                    executor.Run();
                });
                executionThread.Start();
            }
        }
        protected override void Update()
        {
            if (Application.isPlaying && Planner != null)
            {
                base.Update();
                if (Planner.IsNewPlanAvailable())
                {
                    //Debug.Log("New plan");
                    plan = Planner.GetNewPlan();
                    if (plan != null)
                    {
                        plan.priority = Priority;
                        Coordinator.PlanReady(this);
                    }
                }
            }
        }

        public int CompareTo(PlannerBrain other)
        {
            return -Priority.CompareTo(other.Priority);
        }

        internal override string ActualSensorEncoding(string sensorsAsASP)
        {
            if (Planner != null)
            {
                return Planner.ActualSensorEncoding(sensorsAsASP);
            }
            return sensorsAsASP;
        }

        protected override string SpecificFileParts()
        {
            if (Planner != null)
            {
                return Planner.SpecificFileParts();
            }
            string toReturn = "%For ASP programs:\n" + new ASPPlannerBrain().SpecificFileParts();
            return toReturn;
        }
    }
}
