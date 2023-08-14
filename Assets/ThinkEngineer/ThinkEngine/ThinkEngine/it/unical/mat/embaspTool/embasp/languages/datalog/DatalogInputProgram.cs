using System;
using System.Collections.Generic;

namespace it.unical.mat.embasp.languages.datalog
{
    using InputProgram = it.unical.mat.embasp.@base.InputProgram;

    public class DatalogInputProgram : InputProgram
    {
        public DatalogInputProgram() : base() { }

        public DatalogInputProgram(object inputObj) : base(inputObj) { }

        public DatalogInputProgram(string initial_program) : base(initial_program) { }

        public override void AddObjectInput(object inputObj) => AddProgram(DatalogMapper.Instance.GetString(inputObj) + ".");

        public override void AddObjectsInput(ISet<object> inputObjs)
        {
            foreach (Object inputObj in inputObjs)
                AddObjectInput(inputObj);
        }
    }
}