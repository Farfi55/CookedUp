//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from DLVParser.g4 by ANTLR 4.7

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public partial class DLVParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		HEADER=1, COLON=2, COST_BEGIN=3, COST_END=4, OPEN_SQUARE_BRACKET=5, CLOSE_SQUARE_BRACKET=6, 
		GROUND_QUERY_BEGIN=7, MODEL_BEGIN=8, MODEL_END=9, WEIGHTED_MODEL_LABEL=10, 
		COMMA=11, IDENTIFIER=12, INTEGER_CONSTANT=13, STRING_CONSTANT=14, TERMS_BEGIN=15, 
		TERMS_END=16, WHITESPACE=17, REASONING=18, DOT=19, BOOLEAN=20, WHITESPACE_IN_GROUND_QUERY=21, 
		WITNESS_LABEL=22;
	public const int
		RULE_answer_set = 0, RULE_cost = 1, RULE_cost_level = 2, RULE_model = 3, 
		RULE_output = 4, RULE_predicate = 5, RULE_term = 6, RULE_witness = 7;
	public static readonly string[] ruleNames = {
		"answer_set", "cost", "cost_level", "model", "output", "predicate", "term", 
		"witness"
	};

	private static readonly string[] _LiteralNames = {
		null, null, "':'", "'Cost ([Weight:Level]): <'", "'>'", "'['", "']'", 
		"' is '", "'{'", "'}'", "'Best model:'", "','", null, null, null, "'('", 
		"')'", null, null, "'.'", null, null, "', evidenced by'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "HEADER", "COLON", "COST_BEGIN", "COST_END", "OPEN_SQUARE_BRACKET", 
		"CLOSE_SQUARE_BRACKET", "GROUND_QUERY_BEGIN", "MODEL_BEGIN", "MODEL_END", 
		"WEIGHTED_MODEL_LABEL", "COMMA", "IDENTIFIER", "INTEGER_CONSTANT", "STRING_CONSTANT", 
		"TERMS_BEGIN", "TERMS_END", "WHITESPACE", "REASONING", "DOT", "BOOLEAN", 
		"WHITESPACE_IN_GROUND_QUERY", "WITNESS_LABEL"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "DLVParser.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static DLVParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public DLVParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public DLVParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}
	public partial class Answer_setContext : ParserRuleContext {
		public Answer_setContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_answer_set; } }
	 
		public Answer_setContext() { }
		public virtual void CopyFrom(Answer_setContext context) {
			base.CopyFrom(context);
		}
	}
	public partial class SimpleModelContext : Answer_setContext {
		public ModelContext model() {
			return GetRuleContext<ModelContext>(0);
		}
		public SimpleModelContext(Answer_setContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitSimpleModel(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class GroundQueryContext : Answer_setContext {
		public ITerminalNode[] IDENTIFIER() { return GetTokens(DLVParser.IDENTIFIER); }
		public ITerminalNode IDENTIFIER(int i) {
			return GetToken(DLVParser.IDENTIFIER, i);
		}
		public ITerminalNode GROUND_QUERY_BEGIN() { return GetToken(DLVParser.GROUND_QUERY_BEGIN, 0); }
		public ITerminalNode REASONING() { return GetToken(DLVParser.REASONING, 0); }
		public ITerminalNode BOOLEAN() { return GetToken(DLVParser.BOOLEAN, 0); }
		public ITerminalNode DOT() { return GetToken(DLVParser.DOT, 0); }
		public WitnessContext witness() {
			return GetRuleContext<WitnessContext>(0);
		}
		public ITerminalNode[] COMMA() { return GetTokens(DLVParser.COMMA); }
		public ITerminalNode COMMA(int i) {
			return GetToken(DLVParser.COMMA, i);
		}
		public GroundQueryContext(Answer_setContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitGroundQuery(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class WeightedModelContext : Answer_setContext {
		public ModelContext model() {
			return GetRuleContext<ModelContext>(0);
		}
		public CostContext cost() {
			return GetRuleContext<CostContext>(0);
		}
		public ITerminalNode WEIGHTED_MODEL_LABEL() { return GetToken(DLVParser.WEIGHTED_MODEL_LABEL, 0); }
		public WeightedModelContext(Answer_setContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitWeightedModel(this);
			else return visitor.VisitChildren(this);
		}
	}
	public partial class NonGroundQueryContext : Answer_setContext {
		public TermContext[] term() {
			return GetRuleContexts<TermContext>();
		}
		public TermContext term(int i) {
			return GetRuleContext<TermContext>(i);
		}
		public ITerminalNode[] COMMA() { return GetTokens(DLVParser.COMMA); }
		public ITerminalNode COMMA(int i) {
			return GetToken(DLVParser.COMMA, i);
		}
		public NonGroundQueryContext(Answer_setContext context) { CopyFrom(context); }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNonGroundQuery(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Answer_setContext answer_set() {
		Answer_setContext _localctx = new Answer_setContext(Context, State);
		EnterRule(_localctx, 0, RULE_answer_set);
		int _la;
		try {
			State = 46;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,4,Context) ) {
			case 1:
				_localctx = new GroundQueryContext(_localctx);
				EnterOuterAlt(_localctx, 1);
				{
				State = 16; Match(IDENTIFIER);
				State = 21;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				while (_la==COMMA) {
					{
					{
					State = 17; Match(COMMA);
					State = 18; Match(IDENTIFIER);
					}
					}
					State = 23;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
				}
				State = 24; Match(GROUND_QUERY_BEGIN);
				State = 25; Match(REASONING);
				State = 26; Match(BOOLEAN);
				State = 29;
				ErrorHandler.Sync(this);
				switch (TokenStream.LA(1)) {
				case DOT:
					{
					State = 27; Match(DOT);
					}
					break;
				case WITNESS_LABEL:
					{
					State = 28; witness();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			case 2:
				_localctx = new SimpleModelContext(_localctx);
				EnterOuterAlt(_localctx, 2);
				{
				State = 31; model();
				}
				break;
			case 3:
				_localctx = new NonGroundQueryContext(_localctx);
				EnterOuterAlt(_localctx, 3);
				{
				State = 32; term();
				State = 37;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				while (_la==COMMA) {
					{
					{
					State = 33; Match(COMMA);
					State = 34; term();
					}
					}
					State = 39;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
				}
				}
				break;
			case 4:
				_localctx = new WeightedModelContext(_localctx);
				EnterOuterAlt(_localctx, 4);
				{
				State = 41;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if (_la==WEIGHTED_MODEL_LABEL) {
					{
					State = 40; Match(WEIGHTED_MODEL_LABEL);
					}
				}

				State = 43; model();
				State = 44; cost();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class CostContext : ParserRuleContext {
		public ITerminalNode COST_BEGIN() { return GetToken(DLVParser.COST_BEGIN, 0); }
		public Cost_levelContext[] cost_level() {
			return GetRuleContexts<Cost_levelContext>();
		}
		public Cost_levelContext cost_level(int i) {
			return GetRuleContext<Cost_levelContext>(i);
		}
		public ITerminalNode COST_END() { return GetToken(DLVParser.COST_END, 0); }
		public ITerminalNode[] COMMA() { return GetTokens(DLVParser.COMMA); }
		public ITerminalNode COMMA(int i) {
			return GetToken(DLVParser.COMMA, i);
		}
		public CostContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_cost; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitCost(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public CostContext cost() {
		CostContext _localctx = new CostContext(Context, State);
		EnterRule(_localctx, 2, RULE_cost);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 48; Match(COST_BEGIN);
			State = 49; cost_level();
			State = 54;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==COMMA) {
				{
				{
				State = 50; Match(COMMA);
				State = 51; cost_level();
				}
				}
				State = 56;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 57; Match(COST_END);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class Cost_levelContext : ParserRuleContext {
		public ITerminalNode OPEN_SQUARE_BRACKET() { return GetToken(DLVParser.OPEN_SQUARE_BRACKET, 0); }
		public ITerminalNode[] INTEGER_CONSTANT() { return GetTokens(DLVParser.INTEGER_CONSTANT); }
		public ITerminalNode INTEGER_CONSTANT(int i) {
			return GetToken(DLVParser.INTEGER_CONSTANT, i);
		}
		public ITerminalNode COLON() { return GetToken(DLVParser.COLON, 0); }
		public ITerminalNode CLOSE_SQUARE_BRACKET() { return GetToken(DLVParser.CLOSE_SQUARE_BRACKET, 0); }
		public Cost_levelContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_cost_level; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitCost_level(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Cost_levelContext cost_level() {
		Cost_levelContext _localctx = new Cost_levelContext(Context, State);
		EnterRule(_localctx, 4, RULE_cost_level);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 59; Match(OPEN_SQUARE_BRACKET);
			State = 60; Match(INTEGER_CONSTANT);
			State = 61; Match(COLON);
			State = 62; Match(INTEGER_CONSTANT);
			State = 63; Match(CLOSE_SQUARE_BRACKET);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ModelContext : ParserRuleContext {
		public ITerminalNode MODEL_BEGIN() { return GetToken(DLVParser.MODEL_BEGIN, 0); }
		public ITerminalNode MODEL_END() { return GetToken(DLVParser.MODEL_END, 0); }
		public PredicateContext[] predicate() {
			return GetRuleContexts<PredicateContext>();
		}
		public PredicateContext predicate(int i) {
			return GetRuleContext<PredicateContext>(i);
		}
		public ITerminalNode[] COMMA() { return GetTokens(DLVParser.COMMA); }
		public ITerminalNode COMMA(int i) {
			return GetToken(DLVParser.COMMA, i);
		}
		public ModelContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_model; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitModel(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ModelContext model() {
		ModelContext _localctx = new ModelContext(Context, State);
		EnterRule(_localctx, 6, RULE_model);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 65; Match(MODEL_BEGIN);
			State = 74;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==IDENTIFIER) {
				{
				State = 66; predicate();
				State = 71;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				while (_la==COMMA) {
					{
					{
					State = 67; Match(COMMA);
					State = 68; predicate();
					}
					}
					State = 73;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
				}
				}
			}

			State = 76; Match(MODEL_END);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OutputContext : ParserRuleContext {
		public Answer_setContext[] answer_set() {
			return GetRuleContexts<Answer_setContext>();
		}
		public Answer_setContext answer_set(int i) {
			return GetRuleContext<Answer_setContext>(i);
		}
		public OutputContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_output; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOutput(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OutputContext output() {
		OutputContext _localctx = new OutputContext(Context, State);
		EnterRule(_localctx, 8, RULE_output);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 81;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << OPEN_SQUARE_BRACKET) | (1L << MODEL_BEGIN) | (1L << WEIGHTED_MODEL_LABEL) | (1L << IDENTIFIER) | (1L << INTEGER_CONSTANT) | (1L << STRING_CONSTANT))) != 0)) {
				{
				{
				State = 78; answer_set();
				}
				}
				State = 83;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class PredicateContext : ParserRuleContext {
		public ITerminalNode IDENTIFIER() { return GetToken(DLVParser.IDENTIFIER, 0); }
		public ITerminalNode TERMS_BEGIN() { return GetToken(DLVParser.TERMS_BEGIN, 0); }
		public TermContext[] term() {
			return GetRuleContexts<TermContext>();
		}
		public TermContext term(int i) {
			return GetRuleContext<TermContext>(i);
		}
		public ITerminalNode TERMS_END() { return GetToken(DLVParser.TERMS_END, 0); }
		public ITerminalNode[] COMMA() { return GetTokens(DLVParser.COMMA); }
		public ITerminalNode COMMA(int i) {
			return GetToken(DLVParser.COMMA, i);
		}
		public PredicateContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_predicate; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitPredicate(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public PredicateContext predicate() {
		PredicateContext _localctx = new PredicateContext(Context, State);
		EnterRule(_localctx, 10, RULE_predicate);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 84; Match(IDENTIFIER);
			State = 96;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==TERMS_BEGIN) {
				{
				State = 85; Match(TERMS_BEGIN);
				State = 86; term();
				State = 91;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				while (_la==COMMA) {
					{
					{
					State = 87; Match(COMMA);
					State = 88; term();
					}
					}
					State = 93;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
				}
				State = 94; Match(TERMS_END);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TermContext : ParserRuleContext {
		public ITerminalNode IDENTIFIER() { return GetToken(DLVParser.IDENTIFIER, 0); }
		public ITerminalNode INTEGER_CONSTANT() { return GetToken(DLVParser.INTEGER_CONSTANT, 0); }
		public PredicateContext predicate() {
			return GetRuleContext<PredicateContext>(0);
		}
		public ITerminalNode OPEN_SQUARE_BRACKET() { return GetToken(DLVParser.OPEN_SQUARE_BRACKET, 0); }
		public ITerminalNode CLOSE_SQUARE_BRACKET() { return GetToken(DLVParser.CLOSE_SQUARE_BRACKET, 0); }
		public TermContext[] term() {
			return GetRuleContexts<TermContext>();
		}
		public TermContext term(int i) {
			return GetRuleContext<TermContext>(i);
		}
		public ITerminalNode[] COMMA() { return GetTokens(DLVParser.COMMA); }
		public ITerminalNode COMMA(int i) {
			return GetToken(DLVParser.COMMA, i);
		}
		public ITerminalNode STRING_CONSTANT() { return GetToken(DLVParser.STRING_CONSTANT, 0); }
		public TermContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_term; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitTerm(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public TermContext term() {
		TermContext _localctx = new TermContext(Context, State);
		EnterRule(_localctx, 12, RULE_term);
		int _la;
		try {
			State = 114;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,13,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 98; Match(IDENTIFIER);
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 99; Match(INTEGER_CONSTANT);
				}
				break;
			case 3:
				EnterOuterAlt(_localctx, 3);
				{
				State = 100; predicate();
				}
				break;
			case 4:
				EnterOuterAlt(_localctx, 4);
				{
				State = 101; Match(OPEN_SQUARE_BRACKET);
				State = 110;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << OPEN_SQUARE_BRACKET) | (1L << IDENTIFIER) | (1L << INTEGER_CONSTANT) | (1L << STRING_CONSTANT))) != 0)) {
					{
					State = 102; term();
					State = 107;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
					while (_la==COMMA) {
						{
						{
						State = 103; Match(COMMA);
						State = 104; term();
						}
						}
						State = 109;
						ErrorHandler.Sync(this);
						_la = TokenStream.LA(1);
					}
					}
				}

				State = 112; Match(CLOSE_SQUARE_BRACKET);
				}
				break;
			case 5:
				EnterOuterAlt(_localctx, 5);
				{
				State = 113; Match(STRING_CONSTANT);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class WitnessContext : ParserRuleContext {
		public ITerminalNode WITNESS_LABEL() { return GetToken(DLVParser.WITNESS_LABEL, 0); }
		public ModelContext model() {
			return GetRuleContext<ModelContext>(0);
		}
		public WitnessContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_witness; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDLVParserVisitor<TResult> typedVisitor = visitor as IDLVParserVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitWitness(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public WitnessContext witness() {
		WitnessContext _localctx = new WitnessContext(Context, State);
		EnterRule(_localctx, 14, RULE_witness);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 116; Match(WITNESS_LABEL);
			State = 117; model();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x3', '\x18', 'z', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', 
		'\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', '\x5', '\x4', 
		'\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', '\t', '\b', 
		'\x4', '\t', '\t', '\t', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\a', 
		'\x2', '\x16', '\n', '\x2', '\f', '\x2', '\xE', '\x2', '\x19', '\v', '\x2', 
		'\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', 
		'\x5', '\x2', ' ', '\n', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', 
		'\x3', '\x2', '\a', '\x2', '&', '\n', '\x2', '\f', '\x2', '\xE', '\x2', 
		')', '\v', '\x2', '\x3', '\x2', '\x5', '\x2', ',', '\n', '\x2', '\x3', 
		'\x2', '\x3', '\x2', '\x3', '\x2', '\x5', '\x2', '\x31', '\n', '\x2', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\a', '\x3', '\x37', 
		'\n', '\x3', '\f', '\x3', '\xE', '\x3', ':', '\v', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x5', '\x3', '\x5', '\x3', '\x5', 
		'\x3', '\x5', '\a', '\x5', 'H', '\n', '\x5', '\f', '\x5', '\xE', '\x5', 
		'K', '\v', '\x5', '\x5', '\x5', 'M', '\n', '\x5', '\x3', '\x5', '\x3', 
		'\x5', '\x3', '\x6', '\a', '\x6', 'R', '\n', '\x6', '\f', '\x6', '\xE', 
		'\x6', 'U', '\v', '\x6', '\x3', '\a', '\x3', '\a', '\x3', '\a', '\x3', 
		'\a', '\x3', '\a', '\a', '\a', '\\', '\n', '\a', '\f', '\a', '\xE', '\a', 
		'_', '\v', '\a', '\x3', '\a', '\x3', '\a', '\x5', '\a', '\x63', '\n', 
		'\a', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\b', 
		'\x3', '\b', '\x3', '\b', '\a', '\b', 'l', '\n', '\b', '\f', '\b', '\xE', 
		'\b', 'o', '\v', '\b', '\x5', '\b', 'q', '\n', '\b', '\x3', '\b', '\x3', 
		'\b', '\x5', '\b', 'u', '\n', '\b', '\x3', '\t', '\x3', '\t', '\x3', '\t', 
		'\x3', '\t', '\x2', '\x2', '\n', '\x2', '\x4', '\x6', '\b', '\n', '\f', 
		'\xE', '\x10', '\x2', '\x2', '\x2', '\x84', '\x2', '\x30', '\x3', '\x2', 
		'\x2', '\x2', '\x4', '\x32', '\x3', '\x2', '\x2', '\x2', '\x6', '=', '\x3', 
		'\x2', '\x2', '\x2', '\b', '\x43', '\x3', '\x2', '\x2', '\x2', '\n', 'S', 
		'\x3', '\x2', '\x2', '\x2', '\f', 'V', '\x3', '\x2', '\x2', '\x2', '\xE', 
		't', '\x3', '\x2', '\x2', '\x2', '\x10', 'v', '\x3', '\x2', '\x2', '\x2', 
		'\x12', '\x17', '\a', '\xE', '\x2', '\x2', '\x13', '\x14', '\a', '\r', 
		'\x2', '\x2', '\x14', '\x16', '\a', '\xE', '\x2', '\x2', '\x15', '\x13', 
		'\x3', '\x2', '\x2', '\x2', '\x16', '\x19', '\x3', '\x2', '\x2', '\x2', 
		'\x17', '\x15', '\x3', '\x2', '\x2', '\x2', '\x17', '\x18', '\x3', '\x2', 
		'\x2', '\x2', '\x18', '\x1A', '\x3', '\x2', '\x2', '\x2', '\x19', '\x17', 
		'\x3', '\x2', '\x2', '\x2', '\x1A', '\x1B', '\a', '\t', '\x2', '\x2', 
		'\x1B', '\x1C', '\a', '\x14', '\x2', '\x2', '\x1C', '\x1F', '\a', '\x16', 
		'\x2', '\x2', '\x1D', ' ', '\a', '\x15', '\x2', '\x2', '\x1E', ' ', '\x5', 
		'\x10', '\t', '\x2', '\x1F', '\x1D', '\x3', '\x2', '\x2', '\x2', '\x1F', 
		'\x1E', '\x3', '\x2', '\x2', '\x2', ' ', '\x31', '\x3', '\x2', '\x2', 
		'\x2', '!', '\x31', '\x5', '\b', '\x5', '\x2', '\"', '\'', '\x5', '\xE', 
		'\b', '\x2', '#', '$', '\a', '\r', '\x2', '\x2', '$', '&', '\x5', '\xE', 
		'\b', '\x2', '%', '#', '\x3', '\x2', '\x2', '\x2', '&', ')', '\x3', '\x2', 
		'\x2', '\x2', '\'', '%', '\x3', '\x2', '\x2', '\x2', '\'', '(', '\x3', 
		'\x2', '\x2', '\x2', '(', '\x31', '\x3', '\x2', '\x2', '\x2', ')', '\'', 
		'\x3', '\x2', '\x2', '\x2', '*', ',', '\a', '\f', '\x2', '\x2', '+', '*', 
		'\x3', '\x2', '\x2', '\x2', '+', ',', '\x3', '\x2', '\x2', '\x2', ',', 
		'-', '\x3', '\x2', '\x2', '\x2', '-', '.', '\x5', '\b', '\x5', '\x2', 
		'.', '/', '\x5', '\x4', '\x3', '\x2', '/', '\x31', '\x3', '\x2', '\x2', 
		'\x2', '\x30', '\x12', '\x3', '\x2', '\x2', '\x2', '\x30', '!', '\x3', 
		'\x2', '\x2', '\x2', '\x30', '\"', '\x3', '\x2', '\x2', '\x2', '\x30', 
		'+', '\x3', '\x2', '\x2', '\x2', '\x31', '\x3', '\x3', '\x2', '\x2', '\x2', 
		'\x32', '\x33', '\a', '\x5', '\x2', '\x2', '\x33', '\x38', '\x5', '\x6', 
		'\x4', '\x2', '\x34', '\x35', '\a', '\r', '\x2', '\x2', '\x35', '\x37', 
		'\x5', '\x6', '\x4', '\x2', '\x36', '\x34', '\x3', '\x2', '\x2', '\x2', 
		'\x37', ':', '\x3', '\x2', '\x2', '\x2', '\x38', '\x36', '\x3', '\x2', 
		'\x2', '\x2', '\x38', '\x39', '\x3', '\x2', '\x2', '\x2', '\x39', ';', 
		'\x3', '\x2', '\x2', '\x2', ':', '\x38', '\x3', '\x2', '\x2', '\x2', ';', 
		'<', '\a', '\x6', '\x2', '\x2', '<', '\x5', '\x3', '\x2', '\x2', '\x2', 
		'=', '>', '\a', '\a', '\x2', '\x2', '>', '?', '\a', '\xF', '\x2', '\x2', 
		'?', '@', '\a', '\x4', '\x2', '\x2', '@', '\x41', '\a', '\xF', '\x2', 
		'\x2', '\x41', '\x42', '\a', '\b', '\x2', '\x2', '\x42', '\a', '\x3', 
		'\x2', '\x2', '\x2', '\x43', 'L', '\a', '\n', '\x2', '\x2', '\x44', 'I', 
		'\x5', '\f', '\a', '\x2', '\x45', '\x46', '\a', '\r', '\x2', '\x2', '\x46', 
		'H', '\x5', '\f', '\a', '\x2', 'G', '\x45', '\x3', '\x2', '\x2', '\x2', 
		'H', 'K', '\x3', '\x2', '\x2', '\x2', 'I', 'G', '\x3', '\x2', '\x2', '\x2', 
		'I', 'J', '\x3', '\x2', '\x2', '\x2', 'J', 'M', '\x3', '\x2', '\x2', '\x2', 
		'K', 'I', '\x3', '\x2', '\x2', '\x2', 'L', '\x44', '\x3', '\x2', '\x2', 
		'\x2', 'L', 'M', '\x3', '\x2', '\x2', '\x2', 'M', 'N', '\x3', '\x2', '\x2', 
		'\x2', 'N', 'O', '\a', '\v', '\x2', '\x2', 'O', '\t', '\x3', '\x2', '\x2', 
		'\x2', 'P', 'R', '\x5', '\x2', '\x2', '\x2', 'Q', 'P', '\x3', '\x2', '\x2', 
		'\x2', 'R', 'U', '\x3', '\x2', '\x2', '\x2', 'S', 'Q', '\x3', '\x2', '\x2', 
		'\x2', 'S', 'T', '\x3', '\x2', '\x2', '\x2', 'T', '\v', '\x3', '\x2', 
		'\x2', '\x2', 'U', 'S', '\x3', '\x2', '\x2', '\x2', 'V', '\x62', '\a', 
		'\xE', '\x2', '\x2', 'W', 'X', '\a', '\x11', '\x2', '\x2', 'X', ']', '\x5', 
		'\xE', '\b', '\x2', 'Y', 'Z', '\a', '\r', '\x2', '\x2', 'Z', '\\', '\x5', 
		'\xE', '\b', '\x2', '[', 'Y', '\x3', '\x2', '\x2', '\x2', '\\', '_', '\x3', 
		'\x2', '\x2', '\x2', ']', '[', '\x3', '\x2', '\x2', '\x2', ']', '^', '\x3', 
		'\x2', '\x2', '\x2', '^', '`', '\x3', '\x2', '\x2', '\x2', '_', ']', '\x3', 
		'\x2', '\x2', '\x2', '`', '\x61', '\a', '\x12', '\x2', '\x2', '\x61', 
		'\x63', '\x3', '\x2', '\x2', '\x2', '\x62', 'W', '\x3', '\x2', '\x2', 
		'\x2', '\x62', '\x63', '\x3', '\x2', '\x2', '\x2', '\x63', '\r', '\x3', 
		'\x2', '\x2', '\x2', '\x64', 'u', '\a', '\xE', '\x2', '\x2', '\x65', 'u', 
		'\a', '\xF', '\x2', '\x2', '\x66', 'u', '\x5', '\f', '\a', '\x2', 'g', 
		'p', '\a', '\a', '\x2', '\x2', 'h', 'm', '\x5', '\xE', '\b', '\x2', 'i', 
		'j', '\a', '\r', '\x2', '\x2', 'j', 'l', '\x5', '\xE', '\b', '\x2', 'k', 
		'i', '\x3', '\x2', '\x2', '\x2', 'l', 'o', '\x3', '\x2', '\x2', '\x2', 
		'm', 'k', '\x3', '\x2', '\x2', '\x2', 'm', 'n', '\x3', '\x2', '\x2', '\x2', 
		'n', 'q', '\x3', '\x2', '\x2', '\x2', 'o', 'm', '\x3', '\x2', '\x2', '\x2', 
		'p', 'h', '\x3', '\x2', '\x2', '\x2', 'p', 'q', '\x3', '\x2', '\x2', '\x2', 
		'q', 'r', '\x3', '\x2', '\x2', '\x2', 'r', 'u', '\a', '\b', '\x2', '\x2', 
		's', 'u', '\a', '\x10', '\x2', '\x2', 't', '\x64', '\x3', '\x2', '\x2', 
		'\x2', 't', '\x65', '\x3', '\x2', '\x2', '\x2', 't', '\x66', '\x3', '\x2', 
		'\x2', '\x2', 't', 'g', '\x3', '\x2', '\x2', '\x2', 't', 's', '\x3', '\x2', 
		'\x2', '\x2', 'u', '\xF', '\x3', '\x2', '\x2', '\x2', 'v', 'w', '\a', 
		'\x18', '\x2', '\x2', 'w', 'x', '\x5', '\b', '\x5', '\x2', 'x', '\x11', 
		'\x3', '\x2', '\x2', '\x2', '\x10', '\x17', '\x1F', '\'', '+', '\x30', 
		'\x38', 'I', 'L', 'S', ']', '\x62', 'm', 'p', 't',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
