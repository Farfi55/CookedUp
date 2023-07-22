using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions {
    public class TestAction : Action {
        public override State Prerequisite() {

            Debug.Log("TestAction.Prerequisite()"); 
            return State.READY;

        }

        public override void Do() {
            Debug.Log("TestAction.Do()");
        }

        public override bool Done() {
            Debug.Log("TestAction.Done()");
            return true;
        }
    }
}
