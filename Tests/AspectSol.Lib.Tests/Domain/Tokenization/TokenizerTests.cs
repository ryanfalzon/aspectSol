using System.Collections.Generic;
using System.Linq;
using AspectSol.Lib.Domain.Tokenizer;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AspectSol.Lib.Tests.Domain.Tokenization;

[TestClass]
public class TokenizerTests
{
    [TestMethod]
    public void GenerateValidTokensForScript()
    {
	    var expectedOutcome = new List<TokenType>
	    {
		    TokenType.Aspect, TokenType.ArbitraryWord, TokenType.OpenScope,
		    TokenType.AddToDeclaration, TokenType.Wildcard, TokenType.Scope,
		    TokenType.Before, TokenType.ExecutionOf, TokenType.Wildcard, TokenType.FullStop, TokenType.Wildcard, TokenType.Scope,
		    TokenType.Before, TokenType.CallTo, TokenType.Wildcard, TokenType.FullStop, TokenType.ArbitraryWord, TokenType.OpenParenthesis,
		    TokenType.CloseParenthesis, TokenType.Scope,
		    TokenType.After, TokenType.CallTo, TokenType.Wildcard, TokenType.FullStop, TokenType.ArbitraryWord, TokenType.OpenParenthesis,
		    TokenType.CloseParenthesis, TokenType.Scope,
		    TokenType.CloseScope, TokenType.SequenceTerminator
	    };
	    
	    var script = @"
aspect SafeReenentrancy {
	add-to-declaration * ¬{ 
		private bool running = false; 
	}¬

	before execution-of *.* ¬{ 
		require (!running); 
	}¬
	
	before call-to *.transfer() ¬{ 
		running = true; 
	}¬
	
	after call-to *.transfer() ¬{ 
		running = false; 
	}¬
}";
	    var loggerMock = new Mock<ILogger<Tokenizer>>();

        var tokenizer = new Tokenizer(loggerMock.Object);
        var tokens = tokenizer.Tokenize(script);
        
        CollectionAssert.AreEquivalent(expectedOutcome, tokens.Select(x => x.TokenType).ToList());
    }
}