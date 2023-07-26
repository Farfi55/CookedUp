using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using it.unical.mat.embasp.specializations.dlv2;
using UnityEngine;

namespace it.unical.mat.embasp.specializations.incrementalIDLV.desktop
{
    using Output = it.unical.mat.embasp.@base.Output;
    using DesktopService = it.unical.mat.embasp.platforms.desktop.DesktopService;
    using InputProgram = it.unical.mat.embasp.@base.InputProgram;
    using OptionDescriptor = it.unical.mat.embasp.@base.OptionDescriptor;
    using ClingoAnswerSets = it.unical.mat.embasp.specializations.clingo.ClingoAnswerSets;

    public class IncrementalDLV2DesktopService : DesktopService
    {

        string solver_path;
        string error = "";
        public IncrementalDLV2DesktopService(string solver_path, IList<OptionDescriptor> options) : base(solver_path)
        {
            this.solver_path = solver_path;
            RunGrounderProcess(options);
        }

        public void RunGrounderProcess(IList<OptionDescriptor> options)
        {
            try
            {
                string option = "";
                foreach (OptionDescriptor o in options)
                {
                    if (o != null)
                    {
                        option += o.Options;
                        option += o.Separator;
                    }
                    else
                        Console.Error.WriteLine("Warning : wrong " + typeof(OptionDescriptor).FullName);
                }
                option += "--stdin";
                solver_process = new Process();
                solver_process.StartInfo.FileName = @solver_path;
                solver_process.EnableRaisingEvents = true;
                solver_process.StartInfo.UseShellExecute = false;
                solver_process.StartInfo.CreateNoWindow = true;
                solver_process.StartInfo.Arguments = option;
                solver_process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                solver_process.StartInfo.RedirectStandardInput = true;
                solver_process.StartInfo.RedirectStandardOutput = true;
                solver_process.StartInfo.RedirectStandardError = true;
                //process.OutputDataReceived += ProcessOutputDataHandler;
                solver_process.ErrorDataReceived += new DataReceivedEventHandler(ProcessErrorDataHandler);
                solver_process.Start();
                solver_process.BeginErrorReadLine();
                //process.BeginOutputReadLine();
                //process.BeginErrorReadLine();
            }
            catch (Win32Exception e2)
            {
                Console.Error.WriteLine(e2.ToString());
                Console.Error.Write(e2.StackTrace);
            }

        }

        private void ProcessErrorDataHandler(object sender, DataReceivedEventArgs e)
        {
            //GetOutput("", e.Data);
            if (e.Data != null)
            {
                UnityEngine.Debug.Log(e.Data + "    ERROR!");
            }
            error = e.Data;
        }

        private void ProcessOutputDataHandler(object sender, DataReceivedEventArgs e)
        {
            //GetOutput(e.Data,"");
            UnityEngine.Debug.Log(e.Data + "    OK");

        }

        public override void LoadProgram(InputProgram program, bool background)
        {
            try
            {
                if (program != null)
                {
                    StreamWriter writer = solver_process.StandardInput;
                    if (program.FilesPaths.Count > 0)
                    {
                        string toPrint = "<load path=\"" + program.FilesPaths[0] + "\"";
                        if (background)
                        {
                            toPrint += " background=\"true\"";
                        }
                        writer.WriteLine(toPrint + "/>");
                    }
                }
                else
                    Console.Error.WriteLine("Warning : wrong " + typeof(InputProgram).FullName);
            }catch(Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }

        }

        protected internal override Output GetOutput(string output, string error) => new DLV2AnswerSets(output, error);


        public override Output StartSync(IList<InputProgram> programs, IList<OptionDescriptor> options)
        {
            StreamWriter writer = solver_process.StandardInput;

            string solverOutput = "";
            string solverError = "";

            try
            {
                if (programs.Count > 0)
                {
                    writer.WriteLine("<run/>");
                }
                while (error.Equals("") &&!solverOutput.EndsWith("<END>\n") && !solver_process.HasExited)
                {
                    //UnityEngine.Debug.Log(solverOutput);
                    solverOutput += base.solver_process.StandardOutput.ReadLine()+"\n";
                }
                if (!error.Equals(""))
                {
                    UnityEngine.Debug.Log(error);
                    solverError=error;
                }
                else if(solver_process.HasExited)
                {
                    return GetOutput("", ""); 
                }
                return GetOutput(solverOutput.ToString(), solverError.ToString());
            }
            catch (Exception e2)
            {
                UnityEngine.Debug.LogError(e2.ToString());
                UnityEngine.Debug.LogError(e2.StackTrace);
            }

            return GetOutput("", "");
        }


        public override void StopGrounderProcess()
        {
            try
            {
                if (solver_process.HasExited)
                {
                    return;
                }
                UnityEngine.Debug.Log("exiting");
                StreamWriter writer = solver_process.StandardInput;
                writer.WriteLine("<exit/>");
            }
            catch (Win32Exception e2)
            {
                Console.Error.WriteLine(e2.ToString());
                Console.Error.Write(e2.StackTrace);
            }
        }

    }
}