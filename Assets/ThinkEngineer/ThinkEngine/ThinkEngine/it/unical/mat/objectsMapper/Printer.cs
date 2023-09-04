using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace ThinkEngine.it.unical.mat.objectsMapper
{
    [InitializeOnLoad]
    public  class Printer : MonoBehaviour
    {
        public List<ITestingBuild> builds = new List<ITestingBuild>();
        public string testName;
        public MonoScript test;
        public int callbackOrder
        {
            get { return 0; }
        }
        public void OnPreprocessBuild(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                Debug.Log("Invoked");
                using (StreamWriter s = File.CreateText(Path.Combine(".", "Assets", testName + ".cs")))
                {
                    s.Write("using ThinkEngine.it.unical.mat.objectsMapper;\n");
                    s.Write("class " + testName + " : ITestingBuild{\n\tpublic override void Print(){\n\tUnityEngine.Debug.Log(\"It worked!\"); \n\t}\n}");
                }
                UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
            }

        }
        void Reset()
        {
            Awake();
        }
        void Awake()
        {
            EditorApplication.playModeStateChanged += OnPreprocessBuild;
        }
        void Start()
        {
            if (Application.isPlaying)
            {
                builds.Add(ScriptableObject.CreateInstance(testName) as ITestingBuild);
            }
        }
        void Update()
        {
            if (Application.isPlaying)
            {
                foreach (ITestingBuild build in builds)
                {
                    build.Print();
                }
            }
        }
    }
}
