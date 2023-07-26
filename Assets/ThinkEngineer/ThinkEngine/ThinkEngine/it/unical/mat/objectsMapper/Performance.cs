using System;
using UnityEngine;
using System.IO;
using System.Globalization;
using ThinkEngine.it.unical.mat.objectsMapper.BrainsScripts;
using ThinkEngine.Planning;

namespace ThinkEngine
{
    class Performance : MonoBehaviour
    {
        private int steps;
        private static readonly string withBrainPath = Path.Combine("Performance", "withBrain.csv");
        private static readonly string withoutBrainPath = Path.Combine("Performance", "withoutBrain.csv");
        private static readonly string factsAndASPath = Path.Combine("Performance", "factsAndAS.csv");
        private static readonly string sensorsUpdateRate = Path.Combine("Performance", "sensors.csv");
        private static readonly string actuatorsUpdateRate = Path.Combine("Performance", "actuators.csv");
        private static readonly string plansUpdateRate = Path.Combine("Performance", "plans.csv");
        private static string path;
        public static bool updatedSensors;
        public static bool updatingActuators;
        static readonly NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        public static bool initialized;
        internal static object toLock = new object();



        void Start()
        {

            if (!Directory.Exists("Performance"))
            {
                Directory.CreateDirectory("Performance");
            }
            nfi.NumberDecimalSeparator = ".";
            Brain[] brains = FindObjectsOfType<Brain>();
            bool found = false;
            foreach (Brain brain in brains)
            {
                if (brain != null && brain.enableBrain)
                {
                    found = true;
                    using (StreamWriter fs = new StreamWriter(withBrainPath, true))
                    {
                        fs.Write("\nIteration; Rate; AvgRate\n");
                        fs.Close();
                    }
                    path = withBrainPath;

                    if (FindObjectOfType<PlannerBrain>() != null)
                    {
                        using (StreamWriter fs = new StreamWriter(plansUpdateRate, true))
                        {
                            fs.Write("\nIteration; Rate; AvgRate; TotalNumberOfActions \n");
                            fs.Close();
                        }
                    }
                    break;
                }
            }
            if (FindObjectOfType<MonoBehaviourSensorsManager>() != null)
            {
                using (StreamWriter fs = new StreamWriter(sensorsUpdateRate, true))
                {
                    fs.Write("\nIteration; Rate; AvgRate; NumberOfSensors; UpdatedSensors; MSPerIteration; ElapsedMS \n");
                    fs.Close();
                }
            }
            if (FindObjectOfType<MonoBehaviourActuator>() != null)
            {
                using (StreamWriter fs = new StreamWriter(actuatorsUpdateRate, true))
                {
                    fs.Write("\nIteration; Rate\n");
                    fs.Close();
                }
            }
            if (!found)
            {

                using (StreamWriter fs = new StreamWriter(withoutBrainPath, true))
                {
                    fs.Write("\nIteration; Rate; AvgRate\n");
                    fs.Close();
                }
                path = withoutBrainPath;

            }
            initialized = true;
        }
        void LateUpdate()
        {
            double current = 1f / Time.unscaledDeltaTime;
            steps++;
            using (StreamWriter fs = new StreamWriter(path, true))
            {
                fs.Write(steps + ";" + Math.Round(current, 4).ToString("N", nfi) + ";" + Utility.SensorsManager.avgFps + ";" + "\n");

                fs.Close();
            }
            if (FindObjectOfType<MonoBehaviourSensorsManager>() != null && !Executor._canRead)//if Executors can not read, sensors are updating
            {
                using (StreamWriter fs = new StreamWriter(sensorsUpdateRate, true))
                {
                    fs.Write(steps + ";" + Math.Round(current, 4).ToString("N", nfi) + ";" + Utility.SensorsManager.avgFps + ";" + SensorsManager.numberOfSensors + ";" + SensorsManager.updatedSensors + ";" + SensorsManager.MAX_MS + ";" + SensorsManager.MS + "\n");
                    fs.Close();
                }
            }
            if (path.Equals(withBrainPath))
            {
                if (FindObjectOfType<PlannerBrain>() != null)
                {
                    using (StreamWriter fs = new StreamWriter(plansUpdateRate, true))
                    {
                        fs.Write(steps + ";" + Math.Round(current, 4).ToString("N", nfi) + ";" + Utility.SensorsManager.avgFps + ";" + FindObjectOfType<Scheduler>().ResidualActions + "\n");
                        fs.Close();
                    }
                }

                if (updatingActuators)
                {
                    updatingActuators = false;
                    using (StreamWriter fs = new StreamWriter(actuatorsUpdateRate, true))
                    {
                        fs.Write(steps + ";" + Math.Round(current, 4).ToString("N", nfi) + "\n");
                        fs.Close();
                    }
                }
            }
        }

        void OnApplicationQuit()
        {
            /*stopwatch.Stop();

            avgFrames =(int)( totalFrames / steps);
            using (StreamWriter fs = new StreamWriter(path, true))
            {
                if (!initialized)
                {
                    return;
                }
                using (StreamWriter fs = new StreamWriter(factsAndASPath, true))
                {
                    fs.Write(s + " " + d.ToString("N", nfi) + "ms \n");
                    fs.Close();
                }
            }
                fs.Write("Brain: "+(GameObject.FindObjectOfType<Brain>().enableBrain)+" "+(stopwatch.ElapsedMilliseconds/1000)+"s \n");
                fs.Write("Average Rate: "+avgFrames + "; Minimum Rate: "+minRate+"; Maximum Rate: "+maxRate+ "\n");
                fs.Close();
            }*/
        }

        public static void WriteOnFile(string s, double d, bool printDate = false)
        {
            lock (toLock)
            {
                if (!initialized)
                {
                    return;
                }
                using (StreamWriter fs = new StreamWriter(factsAndASPath, true))
                {
                    if (printDate)
                    {
                        fs.Write("date: " + DateTime.Today);
                    }
                    fs.Write(s + " " + d.ToString("N", nfi) + "ms \n");
                    fs.Close();
                }
            }
        }
    }
}