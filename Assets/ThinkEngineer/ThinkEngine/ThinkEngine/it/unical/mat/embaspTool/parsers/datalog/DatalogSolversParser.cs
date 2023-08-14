using it.unical.mat.parsers.datalog.idlv;

namespace it.unical.mat.parsers.datalog
{
    public static class DatalogSolversParser
    {
        public static void ParseIDLV(IDatalogDataCollection minimalModels, string atomsList, bool two_stageParsing)
        {
            IDLVParserBaseVisitorImplementation.Parse(minimalModels, atomsList, two_stageParsing);
        }

    }
}