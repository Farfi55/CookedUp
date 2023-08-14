using System.Collections.Generic;
using System;
using it.unical.mat.embasp.platforms.desktop;
using UnityEngine;

namespace it.unical.mat.embasp.specializations.incrementalIDLV.desktop
{
    using ICallback = it.unical.mat.embasp.@base.ICallback;
    using Handler = it.unical.mat.embasp.@base.Handler;
    using InputProgram = it.unical.mat.embasp.@base.InputProgram;
    using OptionDescriptor = it.unical.mat.embasp.@base.OptionDescriptor;
    using Output = it.unical.mat.embasp.@base.Output;
    using DesktopService = it.unical.mat.embasp.platforms.desktop.DesktopService;

    public class IncrementalDLV2DesktopHandler : Handler
    {
        private readonly DesktopService service;

        public IncrementalDLV2DesktopHandler(DesktopService service) => this.service = service;

        public override int AddProgram(InputProgram program, bool background = false)
        {
            //UnityEngine.Debug.Log("adding program");
            int last_index = programs.Count;
            int current_value = last_index;
            programs[last_index++] = program;
            service.LoadProgram(program, background);
            return current_value;
        }

        public override void Quit()
        {
            service.StopGrounderProcess();
        }

        public override void StartAsync(ICallback c, IList<int> program_index, IList<int> option_index)
        {
            IList<InputProgram> input_programs = CollectPrograms(program_index);
            IList<OptionDescriptor> input_options = CollectOptions(option_index);
            service.StartAsync(c, input_programs, input_options);
        }

        public override Output StartSync(IList<int> program_index, IList<int> option_index)
        {
            IList<InputProgram> input_programs = CollectPrograms(program_index);
            IList<OptionDescriptor> input_options = CollectOptions(option_index);
            return service.StartSync(input_programs, input_options);
        }
        public override void StopProcess()
        {
            try
            {
                if (service.solver_process != null && service.solver_process.StartTime != null && !service.solver_process.HasExited)
                {
                    service.solver_process.Kill();
                }
            }
            catch (Exception e)
            {
                Debug.Log("Tried to kill dlv but exception occurred");
                Debug.Log(e);
            }
        }
    }
}