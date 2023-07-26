using System;
using System.Collections.Generic;

namespace it.unical.mat.embasp.languages.datalog
{
    public class MinimalModel
    {
        private readonly ISet<string> value;
        private ISet<object> atoms;

        public MinimalModel(ISet<string> output)
        {
            value = output;
        }

        public virtual ISet<string> GetAtomsAsStringList => value;


        public virtual ISet<object> Atoms
        {
            get
            {
                if (atoms == null)
                {
                    atoms = new HashSet<object>();
                    DatalogMapper mapper = DatalogMapper.Instance;
                    foreach (String atom in value)
                    {
                        object obj = mapper.GetObject(atom);
                        if (obj != null)
                            atoms.Add(obj);
                    }
                }
                return atoms;
            }
        }

        public override string ToString() => value.ToString();
    }
}