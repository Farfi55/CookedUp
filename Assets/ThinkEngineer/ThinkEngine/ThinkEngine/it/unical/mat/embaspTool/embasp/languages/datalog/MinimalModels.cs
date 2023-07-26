using it.unical.mat.parsers.datalog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace it.unical.mat.embasp.languages.datalog
{
    using Output = it.unical.mat.embasp.@base.Output;

    public abstract class MinimalModels : Output, IDatalogDataCollection
    {
        protected internal ISet<MinimalModel> minimalModels;
        
        public MinimalModels(string @out) : base(@out) { }

        public MinimalModels(string @out, string err) : base(@out, err) { }

        public override object Clone() => base.Clone();

        public virtual IList<MinimalModel> Minimalmodels
        {
            get
            {
                if (minimalModels == null)
                {
                    minimalModels = new HashSet<MinimalModel>();
                    Parse();
                }

                return new ReadOnlyCollection<MinimalModel>(minimalModels.ToList());

            }
        }

        public virtual string MinimalModelsAsString => output;

        public void AddMinimalModel(MinimalModel minMod)
        {
            minimalModels.Add(minMod);
        }


    }
}