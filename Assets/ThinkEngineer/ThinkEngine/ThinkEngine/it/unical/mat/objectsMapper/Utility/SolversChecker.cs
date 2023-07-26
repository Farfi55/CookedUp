using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.platforms.desktop;
using it.unical.mat.embasp.specializations.clingo.desktop;
using it.unical.mat.embasp.specializations.dlv2.desktop;
using it.unical.mat.embasp.specializations.incrementalIDLV.desktop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkEngine.it.unical.mat.objectsMapper.BrainsScripts;
using UnityEditor;
using UnityEngine;
using static ThinkEngine.Brain;

namespace ThinkEngine
{
    internal static class SolversChecker
    {

        internal static Handler GetHandler(Brain brain, out string filePath, IList<OptionDescriptor> options)
        {
            foreach (string fileName in Directory.GetFiles(Path.Combine(Utility.StreamingAssetsContent, "lib")))
            {
                string actualFileName = fileName.Substring(fileName.LastIndexOf(Utility.slash) + 1);
                if (actualFileName.StartsWith(brain.SolverName)  && actualFileName.EndsWith(Utility.RunnableExtension))
                {
                    if (brain.SolverName.Equals("dlv"))
                    {
                        brain.incremental = false;
                        filePath = fileName;
                        return new DesktopHandler(new DLV2DesktopService(fileName));
                    }
                    else if (brain.SolverName.Equals("clingo"))
                    {
                        brain.incremental = false;
                        filePath = fileName;
                        return new DesktopHandler(new ClingoDesktopService(fileName));
                    }
                    else if (brain.SolverName.Equals("incremental_dlv2"))
                    {
                        filePath = fileName;
                        brain.incremental = true;
                        return new IncrementalDLV2DesktopHandler(new IncrementalDLV2DesktopService(fileName, options));
                    }
                }
            }
            throw new Exception("Unable to find ASP solver");
        }

        internal static string GetSolverName(SOLVER solverENUM)
        {
            switch (solverENUM)
            {
                case SOLVER.Clingo: return "clingo";
                case SOLVER.DLV2: return "dlv";
                case SOLVER.Incremental_DLV2: return "incremental_dlv2";
            }
            throw new Exception("Solver not supported");
        }

        internal static List<SOLVER> AvailableSolvers()
        {
            List<SOLVER> availableSolvers = new List<SOLVER>();
            foreach (string fileName in Directory.GetFiles(Path.Combine(Utility.StreamingAssetsContent, "lib")))
            {
                string actualFileName = fileName.Substring(fileName.LastIndexOf(Utility.slash) + 1);
                if (actualFileName.StartsWith("dlv") && actualFileName.EndsWith(Utility.RunnableExtension))
                {
                    availableSolvers.Add(SOLVER.DLV2);
                }
                else if (actualFileName.StartsWith("clingo") && actualFileName.EndsWith(Utility.RunnableExtension))
                {
                    availableSolvers.Add(SOLVER.Clingo);
                }
                else if (actualFileName.StartsWith("incremental_dlv2") && actualFileName.EndsWith(Utility.RunnableExtension))
                {
                    availableSolvers.Add(SOLVER.Incremental_DLV2);
                }
            }
            availableSolvers.Sort();
            return availableSolvers;
        }
        internal static bool CheckSolver(Executor executor, string solverName)
        {
            string[] libContent = Directory.GetFiles(Path.Combine(Utility.StreamingAssetsContent, "lib"));
            if (libContent.Length == 0)
            {
                return false;
            }
            foreach (string filename in libContent)
            {
                string actualFileName = filename.Substring(filename.LastIndexOf(Utility.slash) + 1);
                if (actualFileName.StartsWith(solverName) && actualFileName.EndsWith(Utility.RunnableExtension))
                {
                    return executor.TestSolver();
                }
            }
            return false;
        }
    }
}
