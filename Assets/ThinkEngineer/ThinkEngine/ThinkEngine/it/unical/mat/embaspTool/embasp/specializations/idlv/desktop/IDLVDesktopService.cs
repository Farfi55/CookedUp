using System.Collections.Generic;

namespace it.unical.mat.embasp.specializations.idlv.desktop
{
    using Output = it.unical.mat.embasp.@base.Output;
    using DesktopService = it.unical.mat.embasp.platforms.desktop.DesktopService;
    using it.unical.mat.embasp.@base;

    public class IDLVDesktopService : DesktopService
    {
        public IDLVDesktopService(string exe_path) : base(exe_path)
        {
            load_from_STDIN_option = "--stdin";
        }

        public override Output StartSync(IList<InputProgram> programs, IList<OptionDescriptor> options)
        {
            options.Add(new OptionDescriptor("--t "));
            return base.StartSync(programs, options);
        }

        public void StartAsync(ICallback callback, IList<InputProgram> programs, IList<OptionDescriptor> options)
        {
            base.StartAsync(callback, programs, options);
        }

        protected internal override Output GetOutput(string output, string error) => new IDLVMinimalModels(output, error);
    }
}