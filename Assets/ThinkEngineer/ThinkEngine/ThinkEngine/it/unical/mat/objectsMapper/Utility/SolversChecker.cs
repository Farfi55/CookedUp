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

        internal static Handler GetHandler(Brain brain, out string handlerPath, IList<OptionDescriptor> options)
        {
            foreach (string filePath in Directory.GetFiles(Path.Combine(Utility.StreamingAssetsContent, "lib")))
            {
                string fileName =  Path.GetFileName(filePath).ToLower();
                if (fileName.StartsWith(brain.SolverName) && fileName.EndsWith(Utility.RunnableExtension))
                {
                    if (brain.SolverName.Equals("dlv"))
                    {
                        brain.incremental = false;
                        handlerPath = filePath;
                        return new DesktopHandler(new DLV2DesktopService(filePath));
                    }
                    else if (brain.SolverName.Equals("clingo"))
                    {
                        brain.incremental = false;
                        handlerPath = filePath;
                        return new DesktopHandler(new ClingoDesktopService(filePath));
                    }
                    else if (brain.SolverName.Equals("incremental_dlv2"))
                    {
                        handlerPath = filePath;
                        brain.incremental = true;
                        return new IncrementalDLV2DesktopHandler(new IncrementalDLV2DesktopService(filePath, options));
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
            foreach (string filePath in Directory.GetFiles(Path.Combine(Utility.StreamingAssetsContent, "lib")))
            {
                string fileName = Path.GetFileName(filePath).ToLower();
                if(!fileName.EndsWith(Utility.RunnableExtension))
                    continue;
                
                if (fileName.StartsWith("dlv"))
                {
                    availableSolvers.Add(SOLVER.DLV2);
                }
                else if (fileName.StartsWith("clingo"))
                {
                    availableSolvers.Add(SOLVER.Clingo);
                }
                else if (fileName.StartsWith("incremental_dlv2"))
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
            foreach (string filePath in libContent)
            {
                string fileName = Path.GetFileName(filePath).ToLower();
                if (fileName.StartsWith(solverName) && fileName.EndsWith(Utility.RunnableExtension))
                {
                    return executor.TestSolver();
                }
            }
            return false;
        }
    }
}
