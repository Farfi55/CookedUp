using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;

namespace it.unical.mat.parsers.datalog
{
    public class DatalogParser : DatalogGrammarBaseVisitor<object>
    {
        private static List<string> termList = new List<string>();

        private DatalogParser()
        {

        }

        public static String[] ParseParametersFromAtom(string atom)
        {
            termList.Clear();
            CommonTokenStream tokens = new CommonTokenStream(new DatalogGrammarLexer(CharStreams.fromstring(atom)));
            DatalogGrammarParser parser = new DatalogGrammarParser(tokens);
            DatalogParser visitor = new DatalogParser();
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
            return termList.ToArray();
        }


        public override object VisitTerm(DatalogGrammarParser.TermContext context)
        {
            termList.Add(context.GetText());
            return null;
        }
    }
}