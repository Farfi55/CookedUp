using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThinkEngine.Planning
{
    public abstract class Action : ScriptableObject, IComparable<Action>
    {
        public int order;
        public Plan belongingTO;

        public virtual void Init() { }
        public abstract State Prerequisite();
        public abstract void Do();
        public abstract State Done();
        public virtual void Clean() { }

        public int CompareTo(Action other)
        {
            return order.CompareTo(other.order);
        }
    }
    public enum State { READY, WAIT, SKIP, ABORT };
}
