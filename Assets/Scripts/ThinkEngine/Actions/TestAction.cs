using ThinkEngine.Planning;
using UnityEngine;

namespace ThinkEngine.Actions {
    public class TestAction : Action {

        public int TestInt { get; set; } = 0;
        public string testString { get; set; } = "Test";
        public bool testBool1 { get; set; } = false;
        public bool testBool2 { get; set;} = true;



        private void log() {
            Debug.Log("TestAction.log():\n" +
                "TestInt: " + TestInt + "\n" +
                "testString: " + testString + "\n" +
                "testBool1: " + testBool1 + "\n" +
                "testBool2: " + testBool2 + "\n"
            );
        }


        public override State Prerequisite() {

            Debug.Log("TestAction.Prerequisite()"); 
            log();
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
