using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using it.unical.mat.embasp.languages.datalog;

namespace it.unical.mat.parsers.datalog.idlv
{
    public class IDLVParserBaseVisitorImplementation : IDLVParserBaseVisitor<object>
    {
        private readonly IDatalogDataCollection models;
        private HashSet<String> modelCurrentlyBeingVisited;

        private IDLVParserBaseVisitorImplementation(IDatalogDataCollection models)
        {
            this.models = models;
            this.modelCurrentlyBeingVisited = null;
        }

        public override object VisitMinimal_model(IDLVParser.Minimal_modelContext context)
        {

            modelCurrentlyBeingVisited = new HashSet<String>();
            models.AddMinimalModel(new MinimalModel(modelCurrentlyBeingVisited));

            return VisitChildren(context);
        }


        public override object VisitPredicate_atom(IDLVParser.Predicate_atomContext context)
        {
            modelCurrentlyBeingVisited.Add(context.GetText());

            return null;
        }

        public static void Parse(IDatalogDataCollection minimalModels, string atomsList, bool two_stageParsing)
        {
            CommonTokenStream tokens = new CommonTokenStream(new IDLVLexer(CharStreams.fromstring(atomsList)));
            IDLVParser parser = new IDLVParser(tokens);
            IDLVParserBaseVisitorImplementation visitor = new IDLVParserBaseVisitorImplementation(minimalModels);

            if (!two_stageParsing)
            {
                visitor.Visit(parser.output());

                return;
            }

            parser.Interpreter.PredictionMode = PredictionMode.SLL;

            parser.RemoveErrorListeners();

            parser.ErrorHandler = new BailErrorStrategy();

            try
            {
                visitor.Visit(parser.output());
            }
            catch (SystemException exception)
            {
                if (exception.GetBaseException() is RecognitionException)
                {
                    tokens.Seek(0);
                    parser.AddErrorListener(ConsoleErrorListener<object>.Instance);

                    parser.ErrorHandler = new DefaultErrorStrategy();
                    parser.Interpreter.PredictionMode = PredictionMode.LL;

                    visitor.Visit(parser.output());
                }
            }
        }
    }
}