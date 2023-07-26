using it.unical.mat.parsers.datalog;

namespace it.unical.mat.embasp.specializations.idlv
{
    using MinimalModels = it.unical.mat.embasp.languages.datalog.MinimalModels;

    public class IDLVMinimalModels : MinimalModels
    {
        public IDLVMinimalModels(string models) : base(models) { }

        public IDLVMinimalModels(string @out, string err) : base(@out, err) { }

        protected internal override void Parse()
        {
            DatalogSolversParser.ParseIDLV(this, output, true);
        }
    }
}