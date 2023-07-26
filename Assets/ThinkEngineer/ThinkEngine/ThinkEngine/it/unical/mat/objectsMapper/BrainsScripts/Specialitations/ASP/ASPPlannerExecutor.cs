using it.unical.mat.embasp.@base;
using it.unical.mat.embasp.languages.asp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinkEngine.it.unical.mat.objectsMapper.BrainsScripts;
using UnityEngine;

namespace ThinkEngine.Planning
{
    class ASPPlannerExecutor : ASPExecutor
    {

        public ASPPlannerExecutor(PlannerBrain plannerBrain)
        {
            brain = plannerBrain;
        }

        protected override void SpecificAnswerSetOperations(AnswerSet answer)
        {
            ((PlannerBrain)brain).PlanAvailable(answer);
        }

        protected override bool SpecificFactsRetrieving(Brain brain)
        {
            return true;
        }

        protected override void SpecificFactsWriting(Brain brain, StreamWriter fs)
        {

        }

        protected override List<OptionDescriptor> SpecificOptions()
        {
            List<OptionDescriptor> options = new List<OptionDescriptor>();
            options.Add(new OptionDescriptor("--filter=applyAction/2,actionArgument/3 "));
            return options;
        }
    }
}
